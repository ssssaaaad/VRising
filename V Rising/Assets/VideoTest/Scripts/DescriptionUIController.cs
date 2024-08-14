using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUIController : MonoBehaviour
{
    public Image descriptionImage;

    private void Start()
    {
        descriptionImage.DOFade(0.0f, 0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            
            FadeIn();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            
            FadeOut();
        }
    }

    private void FadeIn()
    {
        descriptionImage.DOFade(1f, 1f);
    }

    public void FadeOut()
    {
        descriptionImage.DOFade(0.0f, 1f);
    }
}
