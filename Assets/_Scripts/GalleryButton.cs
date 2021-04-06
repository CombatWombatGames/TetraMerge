using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class GalleryButton : MonoBehaviour
{
    [SerializeField] GameObject locked;
    [SerializeField] Image preview;

    public void Initialize(bool open, UnityAction call)
    {
        preview.gameObject.SetActive(open);
        GetComponent<ButtonEnhanced>().onClick.AddListener(call);
    }
}