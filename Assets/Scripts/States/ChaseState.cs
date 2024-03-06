using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;
    private float currentResetDestinationTimer = 0;

    private float chaseTimerFrequency = 0.5f;

    private float currentChaseTimer = 0;

    private float chaseDistance;


    public ChaseState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
        this.chaseDistance = context.parent.chaseDistance;
    }

    public override void EnterState()
    {
        context.parent.WalkTowards(context.parent.currentTarget.transform.position);
    }

    public override void ExistState()
    {
        currentChaseTimer = 0;
        currentResetDestinationTimer = 0;

    }

    public override EnemyState.EnemyStateOptions GetNextState()
    {
        return EnemyState.EnemyStateOptions.CHASING;
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

    public override void UpdateState()
    {
        currentResetDestinationTimer += Time.deltaTime;
        currentChaseTimer += Time.deltaTime;
        if (context.parent.distanceFromTarget < context.parent.tempAttackRange) {
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.INAACTION);
        }
        if (currentResetDestinationTimer > chaseTimerFrequency && context.parent.distanceFromTarget > (context.parent.tempAttackRange - 1f)) {
            currentChaseTimer += Time.deltaTime;
            currentResetDestinationTimer = 0;
            context.parent.WalkTowards(context.parent.currentTarget.transform.position);
        }
        if (context.parent.distanceeFromInitialPosition > chaseDistance || context.parent.distanceFromTarget > chaseDistance) {
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.RECALLING);
        }
    }

}
