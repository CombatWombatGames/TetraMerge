using UnityEngine;
using UnityEngine.UI;

public class Rune : MonoBehaviour
{
    [SerializeField] Image image = default;
    [SerializeField] Text description = default;

    public void Initialize(Sprite sprite, string description)
    {
        image.sprite = sprite;
        this.description.text = description;
    }
}
