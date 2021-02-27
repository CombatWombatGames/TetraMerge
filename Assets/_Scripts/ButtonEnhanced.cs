using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEnhanced : Button
{
    public ButtonClickedEvent onPointerDown = new ButtonClickedEvent();
    public ButtonClickedEvent onPointerUp = new ButtonClickedEvent();

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onPointerDown.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointerUp.Invoke();
    }

    protected override void OnDestroy()
    {
        onPointerDown.RemoveAllListeners();
        onPointerUp.RemoveAllListeners();
        onClick.RemoveAllListeners();
        base.OnDestroy();
    }
}