using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageFx : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image.DOFade(0f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
