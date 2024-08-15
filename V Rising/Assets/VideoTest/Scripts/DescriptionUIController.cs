using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator FadeIn(int index)
    {
        for (int i = 0; i < index; i++)
        {
            descriptionImages[i].gameObject.SetActive(false);
        }
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.UI_Sound, null, Vector3.zero, 0);
        descriptionImages[index].DOFade(1f, 1f);
        if(index == 6)
        {
            desctionTime = 20;
        }
        yield return new WaitForSeconds(desctionTime);
        descriptionImages[index].DOFade(0.0f, 1f);
    }

}
