using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Text _Text = default;

    public void OnDrag(PointerEventData eventData)
    {
        //Перестает выполняться, когда курсор мыши за пределами объекта, но не заканчивает Drag! 
        //На телефоне работает как надо
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _Text.text = "Dragging!";
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _Text.text = "Drag me again!";
    }
}
