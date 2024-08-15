using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image hover;
    public Image image;



    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("3");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("4");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("5");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("6");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("7");
    }
}
