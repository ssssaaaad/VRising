using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTooltipUIConrtoller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject tooltip;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("1");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("2");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("3");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("4");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("5");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("들어옴");
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("나감");
        tooltip.SetActive(false);
    }
}
