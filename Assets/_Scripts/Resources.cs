using Array2DEditor;
using UnityEngine;
using UnityEngine.Video;

public class Resources : MonoBehaviour
{
    public Sprite[] TilesList => tilesList;
    [SerializeField] Sprite[] tilesList = default;

    public Array2DBool[] PiecesList => piecesList;
    [SerializeField] Array2DBool[] piecesList = default;

    public Array2DBool[] BonusPiecesList => bonusPiecesList;
    [SerializeField] Array2DBool[] bonusPiecesList = default;

    public Array2DBool[] StagesList => stagesList;
    [SerializeField] Array2DBool[] stagesList = default;

    public Color[] ColorsList => colorsList;
    [SerializeField] Color[] colorsList = default;

    public VideoClip[] Tutorials => tutorials;
    [SerializeField] VideoClip[] tutorials = default;

    public Sprite[] CupsList => cupsList;
    [SerializeField] Sprite[] cupsList = default;
}
