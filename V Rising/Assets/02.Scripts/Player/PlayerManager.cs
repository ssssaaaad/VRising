using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator animator;

    private PlayerMove Move;
    private Cskill Cskill;
    private Qskill Qskill;
    private Rskill Rskill;
    private Tskill Tskill;
    private Eskill Eskill;
    private HP_Scan_Range Fscan;
    private Fblood Fblood;
    private PlayerAttack Attack;
    private Playerstate PState;

    private bool canDash = false;
    private bool canCskill = false;
    private bool canQskill = false;
    private bool canRskill = false;
    private bool canTskill = false;
    private bool canEskill = false;
    private bool canComEskill = false;
    private bool canFblood = false;
    private bool canAttack = false;

    public bool dashing = false;
    public bool cskilling = false;
    public bool qskilling = false;
    public bool rskilling = false;
    public bool tskilling = false;
    public bool eskilling = false;
    public bool comeskilling = false;
    public bool fblooding = false;
    public bool attacking = false;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        Move = GetComponent<PlayerMove>();
        Cskill = GetComponent<Cskill>();
        Qskill = GetComponent<Qskill>();
        Rskill = GetComponent<Rskill>();
        Tskill = GetComponent<Tskill>();
        Eskill = GetComponent<Eskill>();
        Fscan = GetComponentInChildren<HP_Scan_Range>();
        Fblood = GetComponent<Fblood>();
        Attack = GetComponent<PlayerAttack>();

        PState = GetComponent<Playerstate>();
    }

    void Update()
    {
        if (PState.dead)
            return;

        if (!Fblood.dontMove)
            Move.Move();

        CanCheck();     // 사용 가능 여부 확인

        InPut();        // 입력 처리

    }

    public void InPut()
    {
        
        if (Input.GetKeyDown(KeyCode.Q) && CanQskill())
        {
            Qskill.Q();
            QCancel();
        }
        if (Input.GetKeyDown(KeyCode.C) && CanCskill())
        {
            Cskill.C();
            animator.SetTrigger("Skill_C");
            CCancel();
        }
        if (Input.GetKeyDown(KeyCode.R) && CanRskill())
        {  
            Rskill.R();
            RCancel();
        }
        if (Input.GetKeyDown(KeyCode.T) && CanTskill())
        {
            Tskill.T();
            TCancel();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("canEskill = " + canEskill);
            if (CanComEskill())
            {
                Eskill.ComboE();
                ECancel();
            }
            else if (CanEskill())
            {
                Eskill.E();
                animator.SetTrigger("Skill_E");
                ECancel();

                Debug.Log("E눌림");
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && CanFblood())
        {
            Fblood.F();
        }
        if (Input.GetMouseButton(0) && CanAttack())
        {
            Attack.Click();
            ClickCancel();
        }
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
            canEskill = false;
            canComEskill = false;
            canFblood = false;
            canAttack = false;
        }
        else if (cskilling)
        {
            canDash = true;
            canCskill = false;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canEskill = true;
            canComEskill = true;
            canFblood = false;
            canAttack = true;
        }
        else if (eskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canEskill = false;
            canComEskill = false;
            canFblood = false;
            canAttack = false;
        }
        else if (comeskilling)
        {
            canDash = false;
            canCskill = false;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canEskill = false;
            canComEskill = false;
            canFblood = false;
            canAttack = false;
        }
        else if (qskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canEskill = false;
            canComEskill = false;
            canFblood = false;
            canAttack = false;
        }
        else if (rskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canEskill = false;
            canComEskill = false;
            canFblood = false;
            canAttack = false;
        }
        else if (tskilling)
        {
            if (!Tskill.HeadLock())
            {
                canDash = true;
                canCskill = true;
                canQskill = false;
                canRskill = false;
                canTskill = false;
                canEskill = false;
                canComEskill = false;
                canFblood = false;
                canAttack = false;
            }
            else
            {
                canDash = false;
                canCskill = false;
                canQskill = false;
                canRskill = false;
                canTskill = false;
                canEskill = false;
                canComEskill = false;
                canFblood = false;
                canAttack = false;
            }
        }
        else if (fblooding)
        {
            canDash = false;
            canCskill = false;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canEskill = false;
            canComEskill = false;
            canFblood = false;
            canAttack = false;
        }
        else if (attacking)
        {
            canDash = true;
            canCskill = true;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canEskill = true;
            canComEskill = true;
            canFblood = false;
            canAttack = true;
        }
        else
        {
            canDash = true;
            canCskill = true;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canEskill = true;
            canComEskill = true;
            canFblood = true;
            canAttack = true;
        }
    }


    public bool CanDash()
    {
        if (!Move.IsDashCoolTime())
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
    public bool CanEskill()
    {
        if (Eskill.IsECoolTime())
            return false;
        else
            return canEskill;
    }
    public bool CanComEskill()
    {
        if (Eskill.ComboEActive() && canComEskill)
            return true;
        else
            return false;
    }
    public bool CanFblood()
    {
        if (!Fscan.CanF())
            return false;
        else
            return canFblood;
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
        if (tskilling)
        {
            Tskill.CancelTSkill();
        }
        if (eskilling)
        {
            Eskill.CancelECasting();
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
        if (tskilling)
        {
            Tskill.CancelTSkill();
        }
        if (eskilling)
        {
            Eskill.CancelECasting();
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
        if (cskilling)
        {
            Cskill.CancelCasting();
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
    public void TCancel()
    {
        if (rskilling)
        {
            Rskill.CancelRCasting();
        }
        if (cskilling)
        {
            Cskill.CancelCasting();
        }
        if (attacking)
        {
            Attack.CancelAttacking();
        }
    }
    public void ECancel()
    {
        if (rskilling)
        {
            Rskill.CancelRCasting();
        }
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
