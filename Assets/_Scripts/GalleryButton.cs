using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class GalleryButton : MonoBehaviour
{
    [SerializeField] GameObject newBadge = default;
    [SerializeField] Image preview = default;

    public void Initialize(bool open, bool showBadge, UnityAction call)
    {
        preview.gameObject.SetActive(open);
        newBadge.SetActive(showBadge);
        GetComponent<ButtonEnhanced>().onClick.AddListener(call);
    }
}