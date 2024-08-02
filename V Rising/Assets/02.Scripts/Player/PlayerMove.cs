using NHance.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private PlayerManager PM;

    public GameObject model;
    public float playerSpeed_Max = 5f; // 기본속도
    public float playerSpeed = 5f; // 기본속도
    public Animator animator;
    
    public float dashSpeed = 20f; // 대쉬 이동속도
    public float dashDuration = 0.45f; // 대쉬 지속 시간
    public float dashFriction = 2f; // 대쉬 중 감쇠속도
    Camera characterCamera;
    CharacterController cc;
    private Rskill Rskill;
    private Cskill Cskill;
    private bool isDashing = false;
    private bool isCastingRSkill = false; // R 스킬 시전 중 여부
    private bool isCastingCSkill = false; // C 스킬 시전 중 여부
    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private Vector3 dashDirection;
    private float dashEndTime = 0f;
    public float dashCooldown = 8f; // 대쉬 쿨타임 (8초)
    private bool canDash = true; // 대쉬 가능 여부
    private float nextDashTime = 0f; // 다음 대쉬 가능 시간
    //public TextMeshProUGUI cooldownText; // 쿨타임 남은시간 택스트
    public SkillUI skillUI;
    public float gravity = -9.8f;
    float yVelocity = 0;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        Rskill = GetComponent<Rskill>();
        Cskill = GetComponent<Cskill>();
        PM = GetComponent<PlayerManager>();
    }


    private void Awake()
    {
        characterCamera = Camera.main;
    }

    void Update()
    {
        PlayerMoving();
        LookMouseCursor();
        // 대쉬 처리
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            SetDashDirection();
            Vector3 modeolDir = model.transform.InverseTransformDirection(moveDirection);

            //animator.SetFloat("Horizontal", 0);
            float dashDirection_z = modeolDir.z * playerSpeed / playerSpeed_Max;
            if (dashDirection_z < 0)
            {
                dashDirection_z = -1;
            }
            else
            {
                dashDirection_z = 1;
            }
            animator.SetFloat("Vertical", dashDirection_z);
            animator.SetTrigger("Dash");
            StartDash();

        }

        // 대쉬 쿨타임 처리
        if (Time.time > nextDashTime)
        {
            canDash = true;
        }

        if (PM.dashing)
        {
            if (Time.time > dashEndTime)
            {

                EndDash();

            }
            else
            {
                // 대쉬 중 미끄러짐 효과 및 방향 전환 적용
                ApplyDashFriction();
                // 대쉬 중 입력을 반영하여 방향 전환
                SetDashDirection();
            }
        }

    }
    void SetDashDirection()
    {
        // 카메라의 방향을 기준으로 대쉬 방향 설정
        Vector3 forward = characterCamera.transform.forward;
        Vector3 right = characterCamera.transform.right;

        // 카메라의 Y축을 기준으로 방향 벡터를 정렬 (수직 방향은 무시)
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // 입력에 따라 대쉬 방향 설정
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        dashDirection = forward * vertical + right * horizontal;

        if (dashDirection.magnitude == 0) // 입력이 없는 경우 기본 방향을 설정
        {
            dashDirection = forward;
        }

        currentVelocity = dashDirection * dashSpeed;
    }

    void StartDash()
    {

        PM.dashing = true;
        canDash = false; // 대쉬 사용 후 쿨타임 시작
        nextDashTime = Time.time + dashCooldown; // 다음 대쉬 가능 시간 설정
        dashEndTime = Time.time + dashDuration;  // 대쉬 종료 시간 설정

        // 대쉬 방향 설정 (캐릭터가 바라보는 방향)
        currentVelocity = dashDirection * dashSpeed;
        cc.Move(currentVelocity * Time.deltaTime);

        skillUI.coolTimeImage();


        //if (PM.rskilling)
        //{
        //    Rskill.CancelRCasting();
        //}
        //if (PM.cskilling )
        //{
        //    Cskill.CancelCasting();
        //}
    }

    void EndDash()
    {

        isDashing = false;

        

    }

    System.Collections.IEnumerator DashCoroutine(Vector3 dashDirection)
    {
        float dashTime = 0f;

        while (dashTime < dashDuration)
        {
            cc.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        EndDash();
        
    }


    public void ApplyDashFriction()
    {
        currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, dashFriction * Time.deltaTime);
        cc.Move(currentVelocity * Time.deltaTime);
    }

    Plane m_plane;
    public bool canRotate = true;

    public void LookMouseCursor()
    {
        m_plane = new Plane(Vector3.up, model.transform.position);


        Ray ray = characterCamera.ScreenPointToRay(Input.mousePosition);

        float zValue;

        m_plane.Raycast(ray, out zValue);


        Vector3 mouseScreenPosition = Input.mousePosition;

        mouseScreenPosition.z = zValue;

        Vector3 mouseWorldPosition = characterCamera.ScreenToWorldPoint(mouseScreenPosition);


        Vector3 mouseDir = mouseWorldPosition - model.transform.position;
        mouseDir.y = 0;

        if (canRotate)
        {
            model.transform.forward = mouseDir;
        }

        //카메라 방향을 기준으로 캐릭터의 이동 방향을 설정

        /////////
        //Vector3 mouseScreenPosition = Input.mousePosition;
        //mouseScreenPosition.z = characterCamera.nearClipPlane; // 카메라의 가까운 클리핑 평면과의 거리 설정

        //// 화면 좌표를 월드 좌표로 변환
        //Vector3 mouseWorldPosition = characterCamera.ScreenToWorldPoint(mouseScreenPosition);

        //// 디버그 출력
        //Debug.Log("Mouse Screen Position: " + mouseScreenPosition);
        //Debug.Log("Mouse World Position: " + mouseWorldPosition);

        //// 마우스 방향 계산
        //Vector3 mouseDir = mouseWorldPosition - player.transform.position;
        //mouseDir.y = 0; // Y축 회전만 고려

        //// 디버그 출력
        //Debug.Log("Mouse Direction: " + mouseDir);

        //// 마우스 방향으로 회전
        //if (mouseDir != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(mouseDir);
        //    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 1000f); // 10f는 회전 속도 조절
        //}

        ////////
        //var ray = charactercamera.screenpointtoray(input.mouseposition);
        //raycasthit hitresult;
        //if (physics.raycast(ray, out hitresult))
        //{
        //    vector3 mousedir = new vector3(hitresult.point.x, transform.position.y, hitresult.point.z) - transform.position;
        //    go.transform.forward = mousedir;
        //}
    }

    public void SetSpeed(float newspeed)
    {
        playerSpeed = newspeed;
    }

    public bool IsCastingRSkill()
    {
        return isCastingRSkill;
    }

    public bool IsCastingCSkill()
    {
        return isCastingCSkill;
    }


    public void PlayerMoving()
    {
        if (!isDashing && !isCastingRSkill && !isCastingCSkill)
        {
            float ad = Input.GetAxis("Horizontal");
            float ws = Input.GetAxis("Vertical");

            Vector3 dirad = transform.right * ad;
            Vector3 dirws = transform.forward * ws;

            moveDirection = dirad + dirws;

            moveDirection.Normalize();


            moveDirection = characterCamera.transform.TransformDirection(moveDirection);
            moveDirection.y = 0; // Y축 회전만 고려

            moveDirection.Normalize();

            yVelocity += gravity * Time.deltaTime;
            moveDirection.y = yVelocity;


            cc.Move(moveDirection * playerSpeed * Time.deltaTime);

            Vector3 modeolDir = model.transform.InverseTransformDirection(moveDirection);

            animator.SetFloat("Horizontal", modeolDir.x * playerSpeed / playerSpeed_Max);
            animator.SetFloat("Vertical", modeolDir.z * playerSpeed / playerSpeed_Max);
        }

    }
}
