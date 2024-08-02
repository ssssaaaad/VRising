using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tskill : MonoBehaviour
{
    PlayerManager PM;
    PlayerMove PlayerMove;

    public float dashReady = 0.5f;  // 시전 준비시간
    public float cooldownTime = 20f; // 쿨타임 (초)
    public float highSpeed = 10f;   // 돌진 속도
    public float normalSpeed = 5f;  // 기본 속도

    private Vector3 head;       // 시전 방향
    private bool isCoolingDown = false;
    private float cooldownEndTime; // 쿨타임 종료 시간
    private Coroutine ghostTime;

    void Start()
    {
        PM = GetComponent<PlayerManager>();
        PlayerMove = GetComponent<PlayerMove>();
    }


    void Update()
    {
        
    }
}
