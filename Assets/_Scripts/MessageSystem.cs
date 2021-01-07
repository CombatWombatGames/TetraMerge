using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] Message messagePrefab  = default;
    [SerializeField] Transform messageList  = default;

    public void ShowMessage(MessageId messageId)
    {
        Message message = Instantiate(messagePrefab, messageList);
        message.Initialize(messageId);
        AnimationSystem.ShowMessage(message);
    }
}
