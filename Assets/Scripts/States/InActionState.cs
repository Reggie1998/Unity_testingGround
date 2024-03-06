using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActionState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;

    private float currentTimer = 0;

    public InActionState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void EnterState()
    {
        Debug.Log("STARTING ACTION");
    }

    public override void ExistState()
    {
        Debug.Log("LEAVING ACTION");
    }

    public override EnemyState.EnemyStateOptions GetNextState()
    {
        return EnemyState.EnemyStateOptions.INAACTION;
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
        currentTimer += Time.deltaTime;
        if (currentTimer > 2.0f) {
            currentTimer = 0;
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.CHASING);
        }
    }
}
