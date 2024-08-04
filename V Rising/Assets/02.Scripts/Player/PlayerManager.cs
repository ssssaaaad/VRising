using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMove Move;
    private Cskill Cskill;
    private Qskill Qskill;
    private Rskill Rskill;
    private Tskill Tskill;
    private PlayerAttack Attack;

    private bool canDash = false;
    private bool canCskill = false;
    private bool canQskill = false;
    private bool canRskill = false;
    private bool canTskill = false;
    private bool canAttack = false;

    public bool dashing = false;
    public bool cskilling = false;
    public bool qskilling = false;
    public bool rskilling = false;
    public bool tskilling = false;
    public bool attacking = false;


    // Start is called before the first frame update
    void Start()
    {
        Move = GetComponent<PlayerMove>();
        Cskill = GetComponent<Cskill>();
        Qskill = GetComponent<Qskill>();
        Rskill = GetComponent<Rskill>();
        Tskill = GetComponent<Tskill>();
        Attack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Move != null)
            Move.Move();

        CanCheck();     // 사용 가능 여부 확인

        if (Input.GetKeyDown(KeyCode.Q) && CanQskill())
            Qskill.Q();
        if (Input.GetKeyDown(KeyCode.C) && CanCskill())
            Cskill.C();
        if (Input.GetKeyDown(KeyCode.R) && CanRskill())
            Rskill.R();
        if (Input.GetMouseButton(0) && CanAttack())
            Attack.Click();

    }

    public void CanCheck()
    {
        if (dashing)
        {
            canDash = false;
            canCskill = false;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canAttack = false;
        }
        else if (cskilling)
        {
            canDash = true;
            canCskill = false;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canAttack = true;
        }
        else if (qskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canAttack = false;
        }
        else if (rskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canAttack = false;
        }
        else if (attacking)
        {
            canDash = true;
            canCskill = true;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canAttack = true;
        }
        else
        {
            canDash = true;
            canCskill = true;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canAttack = true;
        }
    }


    public bool CanDash()
    {
        if (Move.IsDashCoolTime())
            return false;
        else
            return canDash;
    }
    public bool CanCskill()
    {
        if (Cskill.IsCCoolTime())       // 쿨타임중인가
            return false;
        else
            return canCskill;
    }
    public bool CanQskill()
    {
        if (Qskill.IsQCoolTime())       // 쿨타임중인가
            return false;
        else
            return canQskill;
    }
    public bool CanRskill()
    {
        if (Rskill.IsRCoolTime())       // 쿨타임중인가
            return false;
        else
            return canRskill;
    }
    public bool CanTskill()
    {
        if (Tskill.IsTCoolTime())
            return false;
        else
            return canTskill;
    }
    public bool CanAttack()
    {
        if (Attack.Canattack())        // 공격입력을 받을 수 있나
            return canAttack;
        else
            return false;
    }


    public void SpaceCancel()
    {
        if (cskilling)
        {
            Cskill.CancelCasting();
        }
        if (qskilling)
        {
            Qskill.CancelQSkill();
        }
        if (rskilling)
        {
            Rskill.CancelRCasting();
        }
        if (attacking)
        {
            Attack.CancelAttacking();
        }
    }
    public void CCancel()
    {
        if (qskilling)
        {
            Qskill.CancelQSkill();
        }
        if (rskilling)
        {
            Rskill.CancelRCasting();
        }
        if (attacking)
        {
            Attack.CancelAttacking();
        }
    }
    public void QCancel()
    {
        if (rskilling)
        {
            Rskill.CancelRCasting();
        }
        if (attacking)
        {
            Attack.CancelAttacking();
        }
    }
    public void RCancel()
    {
        if (cskilling)
        {
            Cskill.CancelCasting();
        }
        if (attacking)
        {
            Attack.CancelAttacking();
        }
    }
    public void ClickCancel()
    {
        if (cskilling)
        {
            Cskill.CancelCasting();
        }
    }
}
