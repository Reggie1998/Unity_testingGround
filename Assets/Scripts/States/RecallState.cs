using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallState : BaseState<EnemyState.EnemyStateOptions>
{

    EnemyStateContext context;

    public RecallState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void EnterState()
    {
        context.parent.WalkTowards(context.parent.initialPosition);
    }

    public override void ExistState()
    {

    }

    public override EnemyState.EnemyStateOptions GetNextState()
    {
        return EnemyState.EnemyStateOptions.RECALLING;
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
        if (context.parent.distanceeFromInitialPosition < 4.0f) {
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.IDLING);
        }
    }
}
