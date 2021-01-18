using UnityEngine;
using UnityEngine.UI;

//Displays pieces to player
public class PiecesView : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab = default;
    [SerializeField] Transform[] pieceParents = default;

    PiecesModel piecesModel;
    PlayerProgressionModel playerProgressionModel;
    MessageSystem messageSystem;
    Resources resources;
    PieceController[] nextPieces = new PieceController[3];
    Image[][] nextPiecesImages = new Image[3][];
    Transform[][] nextPiecesTransforms = new Transform[3][];
    Vector3[][] nextPiecesWorldCoordinates = new Vector3[3][];
    readonly int[] rotationLookup = { 2, 5, 8, 1, 4, 7, 0, 3, 6 };
    Sprite[] tiles;

    void Awake()
    {
        tiles = GetComponent<Resources>().TilesList;
        piecesModel = GetComponent<PiecesModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        messageSystem = GetComponent<MessageSystem>();
        resources = GetComponent<Resources>();
        piecesModel.PieceRemoved += HidePiece;
        piecesModel.PiecesGenerated += ShowPieces;
        piecesModel.PieceRotated += RotatePiece;
        piecesModel.CollectionLevelUp += OnCollectionLevelUp;
        playerProgressionModel.BestRuneChanged += OnBestRuneChange;
    }

    void OnDestroy()
    {
        piecesModel.PieceRemoved -= HidePiece;
        piecesModel.PiecesGenerated -= ShowPieces;
        piecesModel.PieceRotated -= RotatePiece;
        piecesModel.CollectionLevelUp -= OnCollectionLevelUp;
        playerProgressionModel.BestRuneChanged -= OnBestRuneChange;
    }

    void SpawnPieces()
    {
        foreach (var parent in pieceParents)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                //Fixes lack of OnDestroy for always inactive objects
                //May cause double unsubscribe, it's ok 
                parent.GetChild(i).GetComponent<PieceController>().Unsubscribe();
                Destroy(parent.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < pieceParents.Length; i++)
        {
            nextPieces[i] = Instantiate(piecePrefab, pieceParents[i]).GetComponent<PieceController>();
            nextPieces[i].Initialize(i, gameObject);
        }
    }

    void RotatePiece(int index)
    {
        var transforms = nextPiecesTransforms[index];
        var targetPositions = new Vector3[transforms.Length];
        for (int i = 0; i < targetPositions.Length; i++)
        {
            targetPositions[i] = nextPiecesWorldCoordinates[index][rotationLookup[i]];
        }
        AnimationSystem.RotatePiece(transforms, targetPositions);
        nextPiecesWorldCoordinates[index] = targetPositions;
    }

    void ShowPieces()
    {
        SpawnPieces();
        InitializePositions();
        for (int i = 0; i < nextPieces.Length; i++)
        {
            ShowPiece(piecesModel.NextPieces[i], nextPiecesImages[i]);
        }
    }

    void OnCollectionLevelUp()
    {
        ShowPieces();
        messageSystem.ShowMessage(MessageId.BaseRuneUpdated);
        int tilesCount = resources.TilesList.Length;
        if (playerProgressionModel.LevelNumber == tilesCount + 1)
        {
            messageSystem.ShowMessage(MessageId.BonusFigureUnlocked);
        }
        else if (playerProgressionModel.LevelNumber == tilesCount * (resources.BonusPiecesList.Length + 1) + 1)
        {
            messageSystem.ShowMessage(MessageId.Victory1, 4f);
            messageSystem.ShowMessage(MessageId.Victory2, 6f);
        }
        else if (playerProgressionModel.LevelNumber > tilesCount && playerProgressionModel.LevelNumber % tilesCount == 1)
        {
            messageSystem.ShowMessage(MessageId.BonusFigureChanged);
        }
    }

    void OnBestRuneChange(int level)
    {
        messageSystem.ShowMessage(MessageId.NewRuneCollected);
    }

    void InitializePositions()
    {
        for (int i = 0; i < nextPieces.Length; i++)
        {
            nextPiecesImages[i] = nextPieces[i].GetComponentsInChildren<Image>();
            var images = nextPiecesImages[i];
            var transforms = new Transform[images.Length];
            var positions = new Vector3[images.Length];
            for (int j = 0; j < positions.Length; j++)
            {
                transforms[j] = images[j].transform;
                positions[j] = images[j].transform.position;
            }
            nextPiecesTransforms[i] = transforms;
            nextPiecesWorldCoordinates[i] = positions;
        }
    }

    void ShowPiece(Piece piece, Image[] slot)
    {
        if (piece.Cells.Length != 0)
        {
            bool[] mask = PieceToMask(piece);
            for (int i = 0; i < 9; i++)
            {
                //TODO LOW Fix duplicating functionality
                slot[i].sprite = tiles[(piece.Cells[0].Level - 1) % tiles.Length];
                slot[i].enabled = mask[i];
            }
        }
    }

    bool[] PieceToMask(Piece piece)
    {
        bool[] mask = new bool[9];
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            mask[(1 - piece.Cells[i].GridCoordinate.y) * 3 + piece.Cells[i].GridCoordinate.x + 1] = true;
        }
        return mask;
    }

    public void ReturnPiece(int index)
    {
        nextPieces[index].transform.localPosition = Vector3.zero;
    }

    void HidePiece(int index)
    {
        nextPieces[index].transform.localPosition = Vector3.zero;
    }

    public void ScalePiece(int index, float scale)
    {
        nextPieces[index].transform.localScale *= scale;
    }
}
