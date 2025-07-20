using UnityEngine;
using NaughtyAttributes;
using UnityEngine.EventSystems;

public class Window_Behaviour : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Barra por donde se arrastra la ventana")]
    public RectTransform windowBar;

    private RectTransform windowRect;
    private Vector2 pointerOffset;
    private bool isDragging = false;

    private void Awake()
    {
        windowRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Verifica si se hizo clic sobre la barra de título
        if (IsPointerOverBar(eventData))
        {
            isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                windowRect,
                eventData.position,
                eventData.pressEventCamera,
                out pointerOffset
            );
        }
        else isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            windowRect.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
        {
            windowRect.localPosition = localPointerPosition - pointerOffset;
        }
    }

    private bool IsPointerOverBar(PointerEventData eventData)
    {
        // Detecta si el puntero está sobre la barra (windowBar)
        return RectTransformUtility.RectangleContainsScreenPoint(windowBar, eventData.position, eventData.pressEventCamera);
    }

}
