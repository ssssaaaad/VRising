using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private PlayerManager PManager;

    private Coroutine startAni;

    private void Start()
    {
        PManager = GetComponentInChildren<PlayerManager>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            print("시작");
            PManager.animator.SetTrigger("GameStart");
            startAni = StartCoroutine(StartAni());
        }

    }

    IEnumerator StartAni()
    {
        yield return new WaitForSeconds(10f);

        PManager.IsStart = true;
    }

}
