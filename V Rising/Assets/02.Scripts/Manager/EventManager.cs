using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public GameObject Player;
    public GameObject Swod;
    public GameObject Gate;
    public GameObject Gate1;
    public GameObject Gate2;
    public GameObject Gate3;

    private PlayerManager PManager;
    private Coroutine startAni;
    //public PlayerAudioListener playerAudioListener;

    public GameObject explosion_Effect;
    public GameObject fire_Effect;
    public GameObject rock_Effect1;

    public GameObject jansang_Effect;
    public Image fadeOut;

    public StartScenceCamera startScenceCamera;

    private void Awake()
    {
        if(instance == null)
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
        startAni = StartCoroutine(GameStarter());

        PManager = GetComponentInChildren<PlayerManager>(true);
    }

    public IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(1);
        startScenceCamera.StartCutScene();
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.StartCut, null, Vector3.zero, 0);
        yield return new WaitForSeconds(5f);
        rock_Effect1.SetActive(true);
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Rock, PManager.transform, Vector3.zero);

        yield return new WaitForSeconds(5f);
        explosion_Effect.SetActive(true);
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Explosion, PManager.transform, Vector3.zero);
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Rock, PManager.transform, Vector3.zero);
        Player.SetActive(true);

        startAni = StartCoroutine(StartAni());
    }

    public IEnumerator StartAni()
    {
        PManager.animator.SetTrigger("GameStart");
        yield return new WaitForSeconds(1.5f);

        float check = 0f;
        bool check_Fire = false;
        bool check_FireSound = false;
        while (check < 3)
        {
            Player.transform.position -= new Vector3(0, 0.025f, 0);
            if (!check_FireSound && check > 1.75)
            {
                check_FireSound = true;
                SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Fire, PManager.transform, Vector3.zero);
            }

            if (!check_Fire && check > 2)
            {
                check_Fire = true;
                fire_Effect.SetActive(true);
            }
            check += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        jansang_Effect.SetActive(false);
        PManager.animator.SetTrigger("Landing");
        yield return new WaitForSeconds(0.5f);

        Swod.SetActive(true);

        PManager.IsStart = true;
        StartCoroutine(PManager.DUIC.FadeIn(0));

        StartCoroutine(FightStart());

        SoundManager.instance.ActiveBGM(Sound.AudioClipName.InplaceBGM);
    }

    public void FadeOut()
    {
        fadeOut.DOColor(new Color(0, 0, 0, 1), 3);
    }

   public IEnumerator FightStart()
    {
        yield return new WaitForSeconds(5f);

        // 철창 움직임

        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Door_SFX, Gate1.transform, Vector3.zero);
        float check = 0;
        while (check < 8)
        {
            Gate.transform.position += new Vector3(0, 0.15f,0);
            Gate1.transform.position += new Vector3(0, 0.15f, 0);
            Gate2.transform.position += new Vector3(0, 0.15f, 0);
            Gate3.transform.position += new Vector3(0, 0.15f, 0);
            check += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(Gate);
        Destroy(Gate1);
        Destroy(Gate2);
        Destroy(Gate3);

        StartCoroutine(PManager.DUIC.FadeIn(1));
    }
}
