using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class EventManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Swod;
    public GameObject Gate;
    public GameObject Gate1;
    public GameObject Gate2;
    public GameObject Gate3;

    private PlayerManager PManager;
    private Coroutine startAni;
    

    private void Start()
    {
        startAni = StartCoroutine(GameStarter());

        PManager = GetComponentInChildren<PlayerManager>(true);
    }

    public IEnumerator GameStarter()
    {
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.StartCut, null, Vector3.zero, 0);
        yield return new WaitForSeconds(15f);

        Player.SetActive(true);

        startAni = StartCoroutine(StartAni());
    }

    public IEnumerator StartAni()
    {
        PManager.animator.SetTrigger("GameStart");
        yield return new WaitForSeconds(1.5f);

        float check = 0f;
        while (check < 3)
        {
            Player.transform.position -= new Vector3(0, 0.025f, 0);


            check += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        PManager.animator.SetTrigger("Landing");
        yield return new WaitForSeconds(0.5f);

        Swod.SetActive(true);

        PManager.IsStart = true;
        StartCoroutine(PManager.DUIC.FadeIn(0));

        StartCoroutine(FightStart());
    }

   public IEnumerator FightStart()
    {
        yield return new WaitForSeconds(5f);

        // 철창 움직임
        float check = 0;
        while (check < 5)
        {
            Gate.transform.position += new Vector3(0, 0.3f,0);
            Gate1.transform.position += new Vector3(0, 0.3f, 0);
            Gate2.transform.position += new Vector3(0, 0.3f, 0);
            Gate3.transform.position += new Vector3(0, 0.3f, 0);
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
