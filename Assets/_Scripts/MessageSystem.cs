using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] Message messagePrefab  = default;
    [SerializeField] Transform messageList  = default;

    public void ShowMessage(MessageId messageId)
    {
        Instantiate(messagePrefab, messageList).Initialize(messageId);
    }
}
