using UnityEngine;

public class Tiles : MonoBehaviour
{
    public Sprite[] TilesList => tilesList;
    [SerializeField] Sprite[] tilesList = default;
}
