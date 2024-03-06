using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();


    [SerializeField]
    protected BaseState<EState> CurrentState;

    protected bool isInTransitioningState = false;
    // Start is called before the first frame update
    void Start()
    {
        CurrentState.EnterState();
    }

    // Update is called once per framea
    void Update()
    {
        // Debug.Log("StateManager");
        // EState nextStateKey = CurrentState.GetNextState();
        // Debug.Log("NextStateKey: " + nextStateKey);
        // if (!isInTransitioningState && nextStateKey.Equals(CurrentState.StateKey)) {
        //     Debug.Log("UpdateState");
        //     CurrentState.UpdateState();
        // } else if (!isInTransitioningState) {
        //     TransitionToState(nextStateKey);
        // }

    }

    public void TransitionToState(EState nextStateKey)
    {
        isInTransitioningState = true;
        CurrentState.ExistState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
        isInTransitioningState = false;
    }

    void OnTriggerEnter(Collider other) { }
    void OnTriggerStay(Collider other) { }
    void OnTriggerExit(Collider other) { }
}
