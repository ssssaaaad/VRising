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
using static UnityEditor.MaterialProperty;
using Unity.VisualScripting;
using Sequence = PrimeTween.Sequence;

[System.Serializable]
public class SkakeType
{
    public Ease shakeEase;
    public float cameraShakeStrength;
    public float shakeMagnitude;
    public float shakeDuration;
    public float offsetX;
    public float offsetY;
    public Ease shakeRotEase;
    public float offsetRotX;
    public float offsetRotY;
    public float startDelay;
}

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance {  get; private set; }

    private new Camera camera;

    [Header("흔들기 유형")]
    [SerializeField] private Ease shakeEase;

    [Header("강도")]
    [SerializeField, Range(0f, 100f)] private float cameraShakeStrength = 0.4f; // 쉐이킹 강도

    [Header("크기")]
    [SerializeField, Range(0f, 10000f)] private float shakeMagnitude = 0.3f;  // 쉐이킹 크기

    [Header("지속 시간")]
    [SerializeField, Range(0f, 10f)] private float shakeDuration = 0.5f;  // 쉐이킹 지속 시간

    [Header("X축 조절")]
    [SerializeField, Range(0f, 10f)] private float offsetX = 0.5f;  // 쉐이킹 지속 시간

    [Header("Y축 조절")]
    [SerializeField, Range(0f, 10f)] private float offsetY = 0.5f;  // 쉐이킹 지속 시간

    [Header("흔들기 회전 유형")]
    [SerializeField] private Ease shakeRotEase;

    [Header("X축 회전 조절")]
    [SerializeField, Range(0f, 10f)] private float offsetRotX = 0.5f;  // 쉐이킹 지속 시간

    [Header("Y축 회전 조절")]
    [SerializeField, Range(0f, 10f)] private float offsetRotY = 0.5f;  // 쉐이킹 지속 시간

    [Space]
    [Header("사용안함")]
    [SerializeField, Range(0f, 1f)] private float smoothTime = 0.1f;  // 쉐이킹의 부드러움 정도

    private Vector3 shakeOffset;  // 쉐이킹 오프셋
    private Vector3 velocity = Vector3.zero;  // 부드럽게 이동하기 위한 변수

    public SkakeType[] SkakeTypeList;

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


        Transform transform = camera.transform;

        float num = cameraShakeStrength * camera.orthographicSize * 0.03f;

        Tween.ShakeLocalPosition(
            transform,
            new ShakeSettings(
                new Vector3(num + offsetX, num + offsetY),
                shakeDuration,
                shakeMagnitude,
                enableFalloff: true,
                shakeEase,
                0f,
                1,
                startDelay,
                0f,
                false))
            .Group(
                Tween.ShakeLocalRotation(
                    transform,
                    new ShakeSettings(
                        new Vector3(offsetRotX, offsetRotY, cameraShakeStrength * 0.6f),
                        shakeDuration,
                        shakeMagnitude,
                        enableFalloff: true,
                        shakeRotEase,
                        0f,
                        1,
                        startDelay,
                        0f,
                        false)));

        //Tween.ShakeLocalPosition(camera.transform, new ShakeSettings(cameraShakeStrength * Vector3.forward, shakeDuration, shakeDuration, enableFalloff: true, Ease.Default, 0f, 1, startDelay, 1f, false));
        //Shake();

    }

    public void ShakeSkillCall(Ease shakeEase, float cameraShakeStrength, float shakeMagnitude, float shakeDuration, float offsetX, float offsetY, 
        Ease shakeRotEase, float offsetRotX, float offsetRotY, float startDelay = 0)
    {

        Transform transform = camera.transform;
        float num = cameraShakeStrength * camera.orthographicSize * 0.03f;

        Tween.ShakeLocalPosition(
            transform,
            new ShakeSettings(
                new Vector3(num + offsetX, num + offsetY),
                shakeDuration,
                shakeMagnitude,
                enableFalloff: true,
                shakeEase,
                0f,
                1,
                startDelay,
                0f,
                false))
            .Group(
                Tween.ShakeLocalRotation(
                    transform,
                    new ShakeSettings(
                        new Vector3(offsetRotX, offsetRotY, cameraShakeStrength * 0.6f),
                        shakeDuration,
                        shakeMagnitude,
                        enableFalloff: true,
                        shakeRotEase,
                        0f,
                        1,
                        startDelay,
                        0f,
                        false)));
     
    }

    public void ShakeSkillCall(int index)
    {
        if (index >= SkakeTypeList.Length)
            return;


        Transform transform = camera.transform;
        float num = SkakeTypeList[index].cameraShakeStrength * camera.orthographicSize * 0.03f;

        Tween.ShakeLocalPosition(
            transform,
            new ShakeSettings(
                new Vector3(num + SkakeTypeList[index].offsetX, num + SkakeTypeList[index].offsetY),
                SkakeTypeList[index].shakeDuration,
                SkakeTypeList[index].shakeMagnitude,
                enableFalloff: true,
                SkakeTypeList[index].shakeEase,
                0f,
                1,
                SkakeTypeList[index].startDelay,
                0f,
                false))
            .Group(
                Tween.ShakeLocalRotation(
                    transform,
                    new ShakeSettings(
                        new Vector3(SkakeTypeList[index].offsetRotX, SkakeTypeList[index].offsetRotY, SkakeTypeList[index].cameraShakeStrength * 0.6f),
                        SkakeTypeList[index].shakeDuration,
                        SkakeTypeList[index].shakeMagnitude,
                        enableFalloff: true,
                        SkakeTypeList[index].shakeRotEase,
                        0f,
                        1,
                        SkakeTypeList[index].startDelay,
                        0f,
                        false)));

    }

    public Sequence Shake(float startDelay = 0)
    {
        
        //Transform transform = camera.transform;

        /*
        if (camera.orthographic)
        {
            float num = cameraShakeStrength * camera.orthographicSize * 0.03f;
            return Tween.ShakeLocalPosition(transform, new ShakeSettings(new Vector3(num, num), duration, shakeDuration, enableFalloff: true, Ease.Default, 0f, 1, startDelay, 1f, false)).Group(Tween.ShakeLocalRotation(transform, new ShakeSettings(new Vector3(0f, 0f, cameraShakeStrength * 0.6f), duration, shakeDuration, enableFalloff: true, Ease.Default, 0f, 1, startDelay, 0f, false)));
        }
         */


        return Sequence.Create(Tween.ShakeLocalRotation(transform, new ShakeSettings(cameraShakeStrength * Vector3.one, shakeDuration, shakeDuration, enableFalloff: true, Ease.Default, 0f, 1, startDelay, 1f, false)));
        

        /*
        Transform transform = camera.transform;
        if (camera.orthographic)
        {
            float num = cameraShakeStrength * camera.orthographicSize * 0.03f;
            return Tween.ShakeLocalPosition(transform, new ShakeSettings(new Vector3(num, num), duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)).Group(ShakeLocalRotation(transform, new ShakeSettings(new Vector3(0f, 0f, strengthFactor * 0.6f), duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)));
        }

        return Sequence.Create(Tween.ShakeLocalRotation(transform, new ShakeSettings(strengthFactor * Vector3.one, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, startDelay, endDelay, useUnscaledTime)));
        */

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

        //return Tween.ShakeCamera(camera, cameraShakeStrength, duration: shakeDuration , frequency: shakeMagnitude, startDelay: startDelay);
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
