using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemy1 : Enemy
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        Die,
    }

    public State state;
    public float range;
    public float damage;

    private float distance; 

    private void ChangeState(State state)
    {
        this.state = state;

        switch (state)
        {

        }
    }

    private void StateUpdate()
    {

        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);
        }

        switch (state)
        {

        }
    }

    private void Move()
    {
        if(target != null)
        {
            if(distance <= range)
            {
                MovePosition(target.position);
            }
            else
            {
                ChangeState(State.Attack);
            }
        }
    }


}
