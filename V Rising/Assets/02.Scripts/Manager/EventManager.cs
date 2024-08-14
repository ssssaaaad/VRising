using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class EventManager : MonoBehaviour
{
    public GameObject Player;

    private PlayerManager PManager;

    private Coroutine startAni;
    

    private void Start()
    {
        startAni = StartCoroutine(GameStarter());

        PManager = GetComponentInChildren<PlayerManager>(true);
    }


    public IEnumerator StartAni()
    {
        PManager.animator.SetTrigger("GameStart");
        yield return new WaitForSeconds(5f);

        PManager.IsStart = true;
    }

    public IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(3f);

        Player.SetActive(true);

        startAni = StartCoroutine(StartAni());
    }

}
