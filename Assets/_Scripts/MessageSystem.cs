using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] Message messagePrefab  = default;
    [SerializeField] Transform messageList  = default;
    Color[] colors;

    private void Awake()
    {
        colors = GetComponent<Resources>().ColorsList;
    }

    public void ShowMessage(MessageId messageId, float duration = 3f)
    {
        Message message = Instantiate(messagePrefab, messageList);
        message.Initialize(messageId, colors[(int)messageId]);
        AnimationSystem.ShowMessage(message, duration);
    }
}
