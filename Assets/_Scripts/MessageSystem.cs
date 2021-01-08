using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] Message messagePrefab  = default;
    [SerializeField] Transform messageList  = default;
    //TODO HIGH Color code for messages!
    public void ShowMessage(MessageId messageId, float duration = 3f)
    {
        Message message = Instantiate(messagePrefab, messageList);
        message.Initialize(messageId);
        AnimationSystem.ShowMessage(message, duration);
    }
}
