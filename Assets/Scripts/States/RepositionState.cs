using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;

    Vector3 preferedPosition;

    public RepositionState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void EnterState()
    {

    }

    public override void ExistState()
    {

    }

    public override EnemyState.EnemyStateOptions GetNextState()
    {
        return EnemyState.EnemyStateOptions.REPOSITIONING;
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

    }
}
