using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InGameOptionUIController : MonoBehaviour
{
    public GameObject optionUI;
    public Image endSprite;
    public Image exitSprite;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnInGameOptionButtonClick(optionUI.gameObject.activeSelf);
        }

        
    }

    public void OnBackButtonClick()
    {
        optionUI.SetActive(false);
    }

    public void OnEndButtonClick()
    {
        Application.Quit();
    }

    private void OnInGameOptionButtonClick(bool onOff)
    {
        if(onOff)
            optionUI.SetActive(false);
        else optionUI.SetActive(true);
    }


}
