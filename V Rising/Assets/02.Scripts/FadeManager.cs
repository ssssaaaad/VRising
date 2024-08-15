using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public Image fadeImage;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        fadeImage.gameObject.SetActive(false);
    }
    public void FadeOut(float time = 2)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(new Color(0, 0, 0, 0), time);
    }

    public void FadeIn(float time = 2)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(new Color(0, 0, 0, 1), time);
    }

    public void SetInactiveFade()
    {
        fadeImage.gameObject.SetActive(false);
    }
}
