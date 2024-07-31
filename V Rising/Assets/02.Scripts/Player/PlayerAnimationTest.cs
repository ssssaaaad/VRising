using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public Animator animator;

    bool cast = false;
    public bool cast_R = false;
    public bool cast_T = false;
    float time = 0;
    public float castingTime = 1;
    public float castingTime_T = 2;
    public float castingTime_R = 1;

    public bool normalAttack = false;
    public int count = 0;

    // Update is called once per frame
    void Update()
    {
        if (cast || cast_T || cast_R)
        {
            time += Time.deltaTime;
        }
        //if (cast && time > castingTime)
        //{
        //    cast = false;
        //    animator.SetTrigger("CancelSkill");
        //}
        if (cast_T && time > castingTime_T)
        {
            cast_T = false;
            animator.SetTrigger("Ative_Skill_T");
            StartCoroutine(a());
        }
        if(cast_R && time > castingTime_R)
        {
            cast_R = false;
            animator.SetTrigger("Ative_Skill_R");
        }



        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!cast_R)
            {
                time = 0;
                cast_R = true;
                animator.SetTrigger("Skill_R");
            }

           
        }
        if (Input.GetKeyDown(KeyCode.J) && cast)
        {
            cast = false;
            animator.SetTrigger("CancelSkill");
        }

    

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!cast)
            {
                time = 0;
                cast = true;
                animator.SetTrigger("Skill_C");
            }


        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!cast)
            {
                time = 0;
                cast = true;
                animator.SetTrigger("Skill_Q");
            }
            else
            {
                cast = false;
                animator.SetTrigger("CancelSkill");
            }


        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!cast)
            {
                time = 0;
                cast_T = true;
                animator.SetTrigger("Skill_T");
            }
            else
            {
                cast_T = false;
                animator.SetTrigger("CancelSkill");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("NormalAttack");
            count++;
            if(count >= 3)
            {
                count = 0;
                normalAttack = false;
                animator.SetBool("NormalAttackCheck", normalAttack);
            }
            else
            {
                normalAttack = true;
                animator.SetBool("NormalAttackCheck", normalAttack);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Death");
            animator.SetBool("Death_Check", true);
        }
    }
    IEnumerator a()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("CancelSkill");
    }
}
