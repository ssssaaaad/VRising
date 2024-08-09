//using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using PrimeTween;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance {  get; private set; }

    private new Camera camera;
    [SerializeField, Range(0f, 1f)] float cameraShakeStrength = 0.4f;

    public float shakeDuration = 0.5f;  // 쉐이킹 지속 시간
    public float shakeMagnitude = 0.3f;  // 쉐이킹 강도
    public float smoothTime = 0.1f;  // 쉐이킹의 부드러움 정도

    private Vector3 shakeOffset;  // 쉐이킹 오프셋
    private Vector3 velocity = Vector3.zero;  // 부드럽게 이동하기 위한 변수

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        camera = Camera.main;
    }

    public void StartShake()
    {
        ShakeTest();

        //StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    public void ShakeTest(float startDelay = 0)
    {
        //camera.DOShakePosition(.2f);
        //camera.DOShakeRotation(.1f);
        Shake();
    }

    public Sequence Shake(float startDelay = 0)
    {

        /*
         * 
            public static Sequence ShakeCamera([NotNull] Camera camera, float strengthFactor, float duration = 0.5f, float frequency = 10f, float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
            {
                Transform transform = camera.transform;
                if (camera.orthographic)
                {
                    float num = strengthFactor * camera.orthographicSize * 0.03f;
                    return ShakeLocalPosition(transform, new ShakeSettings(new Vector3(num, num), duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)).Group(ShakeLocalRotation(transform, new ShakeSettings(new Vector3(0f, 0f, strengthFactor * 0.6f), duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)));
                }

                return Sequence.Create(ShakeLocalRotation(transform, new ShakeSettings(strengthFactor * Vector3.one, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)));
            }
        */

        return Tween.ShakeCamera(camera, cameraShakeStrength, startDelay: startDelay);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 originalPos = camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0);

            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, originalPos + shakeOffset, ref velocity, smoothTime);

            elapsed += Time.deltaTime;

            yield return null;
        }
        
        camera.transform.localPosition = Vector3.Lerp(camera.transform.position, originalPos, .3f); ;
        camera.GetComponent<MainCamera>().isCameraShake = false;

    }


}
