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
        {
            Move.Move();
        }
        
        if (dashing)
        {
            canDash = false;
            canCskill = false;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canAttack = false;


            // 대쉬 사용시 캔슬
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
        else if (cskilling)
        {
            canDash = true;
            canCskill = false;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canAttack = true;

            // C스킬 사용시 캔슬
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
        else if (qskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canAttack = false;

            // Q스킬 사용시 캔슬
            if (rskilling)
            {
                Rskill.CancelRCasting();
            }
            if (attacking)
            {
                Attack.CancelAttacking();
            }
        }
        else if (rskilling)
        {
            canDash = true;
            canCskill = true;
            canQskill = false;
            canRskill = false;
            canTskill = false;
            canAttack = false;

            // R스킬 사용시 캔슬
            if (cskilling)
            {
                Cskill.CancelCasting();
            }
            if (attacking)
            {
                Attack.CancelAttacking();
            }
        }
        else if (attacking)
        {
            canDash = true;
            canCskill = true;
            canQskill = true;
            canRskill = true;
            canTskill = true;
            canAttack = true;

            // 클릭 사용시 캔슬
            if (cskilling)
            {
                Cskill.CancelCasting();
            }
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
        return canDash;
    }
    public bool CanCskill()
    {
        return canCskill;
    }
    public bool CanQskill()
    {
        return canQskill;
    }
    public bool CanRskill()
    {
        return canRskill;
    }
    public bool CanTskill()
    {
        return canTskill;
    }
    public bool CanAttack()
    {
        return canAttack;
    }

}
