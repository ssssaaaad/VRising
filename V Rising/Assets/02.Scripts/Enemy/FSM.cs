using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState
{
    protected Enemy monster;

    protected MonsterBaseState(Enemy monster)
    {
        this.monster = monster;
    }

    public abstract void StateStart();

    public abstract void StateUpdate();

    public abstract void StateEnd();
}
public class IdleState : MonsterBaseState
{
    private Maja maja;

    public IdleState(Maja monster) : base(monster)
    {
        this.maja = monster;
    }

    public override void StateEnd()
    {
        return;
    }

    public override void StateStart()
    {
        return;
    }

    public override void StateUpdate()
    {
        return;
    }
}
public class MoveState : MonsterBaseState
{
    private Maja maja;

    public MoveState(Maja monster) : base(monster)
    {
        this.maja = monster;
    }

    public override void StateEnd()
    {
        return;
    }

    public override void StateStart()
    {
        return;
    }

    public override void StateUpdate()
    {
        maja.animator.SetBool("Walk", true);
        maja.Move(Vector3.zero);
        return;
    }
}
public class AttackState : MonsterBaseState
{
    private Maja maja;

    public AttackState(Maja monster) : base(monster)
    {
        this.maja = monster;
    }

    public override void StateEnd()
    {
        return;
    }

    public override void StateStart()
    {
        maja.StopMoveTarget();
    }

    public override void StateUpdate()
    {
        maja.Attack();
    }
}
public class RunawayState : MonsterBaseState
{
    private Maja maja;

    public RunawayState(Maja monster) : base(monster)
    {
        this.maja = monster;
    }

    public override void StateEnd()
    {
        return;
    }

    public override void StateStart()
    {
        return;
    }

    public override void StateUpdate()
    {
        maja.animator.SetBool("Walk", true);
        maja.Runaway();
        return;
    }
}
public class TeleportState : MonsterBaseState
{
    private Maja maja;

    public TeleportState(Maja monster) : base(monster)
    {
        this.maja = monster;
    }

    public override void StateEnd()
    {
        return;
    }

    public override void StateStart()
    {
        maja.animator.SetBool("Walk", false);
        maja.animator.SetTrigger("Teleport");
        maja.StopMoveTarget();
        maja.teleport.ActivePattern(Vector3.zero);
    }

    public override void StateUpdate()
    {

    }
}
public class DeathtState : MonsterBaseState
{
    private Maja maja;

    public DeathtState(Maja monster) : base(monster)
    {
        this.maja = monster;
    }

    public override void StateEnd()
    {
        maja.ActiveDeathState();
    }

    public override void StateStart()
    {
        maja.animator.SetBool("Walk", false);
        maja.animator.SetTrigger("Teleport");
        maja.StopMoveTarget();
        maja.teleport.ActivePattern(Vector3.zero);
    }

    public override void StateUpdate()
    {
        maja.Death();
    }
}

public class FSM 
{
    private MonsterBaseState currentState;
    
    public FSM(MonsterBaseState initState)
    {
        currentState = initState;
        ChangeState(currentState);
    }

    public void ChangeState(MonsterBaseState changeState)
    {
        if(changeState == currentState)
            return;
        if (currentState != null)
        {
            currentState.StateEnd();
        }
        currentState = changeState;
        currentState.StateStart();
    }

    public void UpdateState()
    {
        if (currentState == null)
            return;

        currentState.StateUpdate();
    }
}
