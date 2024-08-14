using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUIController : MonoBehaviour
{
    public float desctionTime = 2f;

    public List<Sprite> descriptionSprites = new List<Sprite>();

    public List<Image> descriptionImages = new List<Image>();

    private void Start()
    {
        for (int i = 0; i < descriptionImages.Count; i++)
        {
            descriptionImages[i].sprite = descriptionSprites[i];
            descriptionImages[i].DOFade(0.0f, 0f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(FadeIn(0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(FadeIn(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(FadeIn(2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(FadeIn(3));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(FadeIn(4));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(FadeIn(5));
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(FadeIn(6));
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartCoroutine(FadeIn(7));
        }
    }

    public IEnumerator FadeIn(int index)
    {
        descriptionImages[index].DOFade(1f, 1f);
        yield return new WaitForSeconds(desctionTime);
        descriptionImages[index].DOFade(0.0f, 1f);

    }

}
