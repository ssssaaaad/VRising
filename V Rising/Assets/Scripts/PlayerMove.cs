using NHance.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5f; // 기본속도
    
    public float dashSpeed = 20f; // 대쉬 이동속도
    public float dashDuration = 0.45f; // 대쉬 지속 시간
    public float dashFriction = 2f; // 대쉬 중 감쇠속도
    Camera characterCamera;
    public GameObject go;
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
    
    public float gravity = -20f;
    float yVelocity = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Rskill = GetComponent<Rskill>();
        Cskill = GetComponent<Cskill>();
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
             StartDash();
            
        }

        // 대쉬 쿨타임 처리
        if (Time.time > nextDashTime)
        {
            canDash = true;
        }

        if (isDashing)
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

        if (!isDashing && !isCastingRSkill && !isCastingCSkill)
        {
            // 일반적인 이동 처리
            cc.Move(moveDirection * Time.deltaTime);
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

        isDashing = true;
        canDash = false; // 대쉬 사용 후 쿨타임 시작
        nextDashTime = Time.time + dashCooldown; // 다음 대쉬 가능 시간 설정
        dashEndTime = Time.time + dashDuration;  // 대쉬 종료 시간 설정

        // 대쉬 방향 설정 (캐릭터가 바라보는 방향)
        currentVelocity = dashDirection * dashSpeed;
        cc.Move(currentVelocity * Time.deltaTime);

        if (Rskill != null && Rskill.IsCasting())
        {
            Rskill.CancelRCasting();
        }
        if (Cskill != null && Cskill.IsCasting())
        {
            Cskill.CancelCasting();
        }
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

    public void LookMouseCursor()
    {


        var ray = characterCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;
        if (Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            go.transform.forward = mouseDir;
        }
    }

    public void SetSpeed(float newspeed)
    {

        playerSpeed = newspeed;
        
    }
    public bool IsDashing()
    {
        return isDashing;
    }

    public bool IsCastingRSkill()
    {
        return isCastingRSkill;
    }

    public bool IsCastingCSkill()
    {
        return isCastingCSkill;
    }

    public void SetCastingRSkill(bool value)
    {
        isCastingRSkill = value;
    }

    public void SetCastingCSkill(bool value)
    {
        isCastingCSkill = value;
    }


    public void PlayerMoving()
    {
        if (!isDashing && !isCastingRSkill && !isCastingCSkill)
        {
            float ad = Input.GetAxis("Horizontal");
            float ws = Input.GetAxis("Vertical");

            Vector3 dirad = transform.right * ad;
            Vector3 dirws = transform.forward * ws;

            Vector3 dir = dirad + dirws;

            dir.Normalize();


            dir = Camera.main.transform.TransformDirection(dir);

            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;
            cc.Move(dir * playerSpeed * Time.deltaTime);

        }
        
    }
}
