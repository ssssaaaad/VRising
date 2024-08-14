using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinMove : MonoBehaviour
{
    //transform.DOMove(transform.position + new Vector3(0, 10, 0), .5f)

    private void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOPunchPosition(new Vector3(1, 0, 1), 5f, 10, 0f, false))
            .Append(transform.DOMove(transform.position + new Vector3(0, 10, 0), .5f))
            .Append(transform.DORotate(new Vector3(90, 180, 0), 5.0f, RotateMode.Fast))
            .Append(transform.DOPunchPosition(new Vector3(1, 0, 1), 5f, 10, 0f, false))
            .OnComplete(() => { 
                transform.gameObject.SetActive(false);
            });


    }
}
