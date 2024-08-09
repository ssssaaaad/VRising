using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Enemy
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        Die
    }

    public State state;

    void Update()
    {
        
    }

    private void ChangeState(State changeState)
    {
        state = changeState;
    }

    private void StateCycle()
    {
        switch (state)
        {

        }
    }
}
