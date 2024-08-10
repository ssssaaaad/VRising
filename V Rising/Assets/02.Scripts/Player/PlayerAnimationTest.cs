using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public PlayerMove playerMove;
    public Animator animator;
    public Transform model;

    bool cast = false;
    public bool cast_R = false;
    public bool cast_T = false;
    public bool cast_C = false;
    float time = 0;
    public float castingTime_T = 2;
    public float castingTime_R = 1;
    public float castingTime_C = 1;

    public bool normalAttack = false;
    public int count = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(cast_R && time > castingTime_R)
        {
            cast_R = false;
            animator.SetTrigger("Ative_Skill_R");
        }
        if (cast_C && time > castingTime_C)
        {
            cast_C = false;
            animator.SetTrigger("CancelSkill");
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
            if (!cast_C)
            {
                time = 0;
                cast_C = true;
                animator.SetTrigger("Skill_C");
            }
            else
            {
                cast_C = false;
                animator.SetTrigger("CancelSkill");
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

        if (Input.GetMouseButtonDown(0))
        {
            if (count == 1)
            {
                normalAttack = true;
                animator.SetBool("NormalAttackCheck", normalAttack);
            }
            else if(count == 0)
            {
                normalAttack = false;
                animator.SetBool("NormalAttackCheck", normalAttack);
            }

            animator.SetTrigger("NormalAttack");
            count++;
            if(count >= 3)
            {
                count = 0;
                animator.SetTrigger("Spine_WholeBody");
                //StartCoroutine(r());
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

    public float rotationStartTime = 0.2f;
    //IEnumerator r()
    //{
    //    yield return new WaitForSeconds(rotationStartTime);
    //    playerMove.canRotate = false;
    //    for (int i = 0; i < 20; i++)
    //    {
    //        model.transform.Rotate(0, 360 / 20, 0);
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //    yield return new WaitForSeconds(rotationStartTime);
    //    playerMove.canRotate = true;
    //}
}
