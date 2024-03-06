using UnityEngine;
using System;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key)
    {
        StateKey = key;
    }

    public EState StateKey { get; private set; }

    public abstract void EnterState();
    public abstract void ExistState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter();
    public abstract void OnTriggerStay();
    public abstract void onTriggerExit();

}