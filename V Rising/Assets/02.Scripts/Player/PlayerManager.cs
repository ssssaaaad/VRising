using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMove Dash;
    private Cskill Cskill;
    private Rskill Rskill;
    private PlayerAttack Attack;

    private bool canDash = false;
    private bool canCskill = false;
    private bool canRskill = false;
    private bool canAttack = false;

    public bool dashing = false;
    public bool cskilling = false;
    public bool rskilling = false;
    public bool attacking = false;

    public enum EAstate
    {
        DASH,
        CSKILL,
        RSKILL,
        ATTACK
    }

    public EAstate CanI;

    // Start is called before the first frame update
    void Start()
    {
        Dash = GetComponent<PlayerMove>();
        Cskill = GetComponent<Cskill>();
        Rskill = GetComponent<Rskill>();
        Attack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing)
        {
            canDash = false;
            canCskill = false;
            canRskill = false;
            canAttack = false;
        }

        if (cskilling)
        {
            canDash = true;
            canCskill = false;
            canRskill = true;
            canAttack = true;
        }

        if (rskilling)
        {
            canDash = true;
            canCskill = true;
            canRskill = false;
            canAttack = false;
        }

        if (attacking)
        {
            canDash = true;
            canCskill = true;
            canRskill = true;
            canAttack = false;
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
    public bool CanRskill()
    {
        return canRskill;
    }
    public bool CanAttack()
    {
        return canAttack;
    }

}
