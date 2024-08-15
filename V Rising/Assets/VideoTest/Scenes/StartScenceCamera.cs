using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartScenceCamera : MonoBehaviour
{
    public Camera playerCamera;
    public Camera startCamera;
    public GameObject coffin;

    public Vector3 pos;
    public Quaternion rot;

    public float disance = 150f;

    public Vector3 endPos;
    public Vector3 endRot;

    private void Start()
    {
        transform.position = pos;
        transform.rotation = rot;
        startCamera.depth = 10f;

        /*
            DOShakePosition(float duration, float/Vector3 strength, int vibrato, float randomness, bool snapping, bool fadeOut, ShakeRandomnessMode randomnessMode)


            DOShakeRotation(float duration, float / Vector3 strength, int vibrato, float randomness, bool fadeOut, ShakeRandomnessMode randomnessMode)
        */

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(transform.position + new Vector3(0, 0, disance), 5f))
            //.Append(coffin.transform.DOShakePosition(2f,new Vector3(Random.Range(-.8f,.8f), Random.Range(-.2f, .2f), Random.Range(-.8f, .8f)), 30, 30, false, false, ShakeRandomnessMode.Harmonic))
            //.Join(transform.DOShakePosition(2f,new Vector3(Random.Range(-.8f,.8f), Random.Range(-.2f, .2f), Random.Range(-.8f, .8f)), 30, 30, false, false, ShakeRandomnessMode.Harmonic))
            .Append(coffin.transform.DOMove(coffin.transform.position + new Vector3(0, 10, 0), .5f))
            .Append(coffin.transform.DORotate(new Vector3(90, 180, 0), 4.0f, RotateMode.Fast))
            //.Append(coffin.transform.DOShakePosition(2f, new Vector3(Random.Range(-.8f, .8f), Random.Range(-.2f, .2f), Random.Range(-.8f, .8f)), 30, 30, false, false, ShakeRandomnessMode.Harmonic))
            //.Join(transform.DOMove(endPos, 3f)).Join(transform.DORotate(endRot,3f,RotateMode.Fast))
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                coffin.transform.gameObject.SetActive(false);

                startCamera.depth = -10f;
                startCamera.gameObject.SetActive(false);
                startCamera.enabled = false;
            });


    }

}
