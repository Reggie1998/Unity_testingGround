using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;
    private float currentWaitTime = 0;

    public IdleState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void UpdateState()
    {
        currentWaitTime += Time.deltaTime;
        if (context.parent.IsEnemyInSight()) {
            Debug.Log("Transition to Chasing State");
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.CHASING);
        }
        if (currentWaitTime > 3) {
            currentWaitTime = 0;
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.WALKING);
            Debug.Log("Transition to Walking State");
        }
    }

    public override void EnterState()
    {
        currentWaitTime = 0;
        Debug.Log("Enter Idling State");
    }

    public override void ExistState()
    {
        currentWaitTime = 0;
    }


    //why do we need this?
    public override EnemyState.EnemyStateOptions GetNextState()
    {
        return EnemyState.EnemyStateOptions.IDLING;
    }

    public override void OnTriggerEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void onTriggerExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay()
    {
        throw new System.NotImplementedException();
    }
}
