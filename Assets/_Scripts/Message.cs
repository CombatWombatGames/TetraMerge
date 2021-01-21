using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] Button button = default;

    public Text Text => text;
    [SerializeField] Text text = default;

    public Image Background => background;
    [SerializeField] Image background = default;

    public void Initialize(MessageId messageId, Color color)
    {
        text.text = Consts.Messages[messageId];
        text.color = color;
    }

    private void Awake()
    {
        button.onClick.AddListener(() => AnimationSystem.HideMessage(this));
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
