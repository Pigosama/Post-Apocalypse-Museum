using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public sealed class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform panel;
    private Canvas parentCanvas;

    private void Awake()
    {
        panel = (RectTransform)transform;
        parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas == null)
        {
            Debug.LogError("DraggablePanel requires a parent Canvas.", this);
            enabled = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        panel.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }
}
