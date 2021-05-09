using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class GalleryButton : MonoBehaviour
{
    [SerializeField] GameObject locked;
    [SerializeField] GameObject newBadge;
    [SerializeField] Image preview;

    public void Initialize(bool open, bool showBadge, UnityAction call)
    {
        preview.gameObject.SetActive(open);
        newBadge.SetActive(showBadge);
        GetComponent<ButtonEnhanced>().onClick.AddListener(call);
    }
}