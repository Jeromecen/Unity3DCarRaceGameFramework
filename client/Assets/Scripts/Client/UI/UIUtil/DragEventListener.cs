using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragEventListener : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public delegate void DragEventDelegate(PointerEventData eventData);
    public DragEventDelegate onDrag = null; 
    public DragEventDelegate onBeginDrag = null;
    public DragEventDelegate onEndDrag = null;
    public  void OnDrag(PointerEventData eventData) {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null)
        {
            onBeginDrag(eventData);
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null)
        {
            onEndDrag(eventData);
        }
    }

}
