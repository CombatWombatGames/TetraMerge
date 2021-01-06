using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] Text text = default;

    public void Initialize(MessageId messageId)
    {
        text.text = Consts.Messages[messageId];
    }
}
