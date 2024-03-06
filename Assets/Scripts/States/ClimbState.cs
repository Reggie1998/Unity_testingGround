using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;

    public ClimbState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExistState()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyState.EnemyStateOptions GetNextState()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
