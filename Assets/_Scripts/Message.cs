using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text Text => text;
    [SerializeField] Text text = default;

    public Image Background => background;
    [SerializeField] Image background = default;

    public void Initialize(MessageId messageId)
    {
        text.text = Consts.Messages[messageId];
    }
}
