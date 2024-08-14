using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScenceCamera : MonoBehaviour
{
    public Camera playerCamera;
    public Camera startCamera;
    public GameObject coffin;

    public Vector3 pos;
    public Quaternion rot;



    private void Start()
    {
        transform.position = pos;
        transform.rotation = rot;
        playerCamera.enabled = false;
        startCamera.enabled = true;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(transform.position + new Vector3(0, 0, 175), 5f))
            .Append(coffin.transform.DOPunchPosition(new Vector3(1, 0, 1), 5f, 10, 0f, false))
            .Append(coffin.transform.DOMove(coffin.transform.position + new Vector3(0, 10, 0), .5f))
            .Append(coffin.transform.DORotate(new Vector3(90, 180, 0), 5.0f, RotateMode.Fast))
            .Append(coffin.transform.DOPunchPosition(new Vector3(1, 0, 1), 5f, 10, 0f, false))
            .OnComplete(() =>
            {
                coffin.transform.gameObject.SetActive(false);
                
                startCamera.enabled = false;
                playerCamera.enabled = true;
            });


    }

}
