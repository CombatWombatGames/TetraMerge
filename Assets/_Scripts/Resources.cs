using Array2DEditor;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public Sprite[] TilesList => tilesList;
    [SerializeField] Sprite[] tilesList = default;

    public Array2DBool[] PiecesList => piecesList;
    [SerializeField] Array2DBool[] piecesList = default;

    public Array2DBool[] BonusPiecesList => bonusPiecesList;
    [SerializeField] Array2DBool[] bonusPiecesList = default;
}
