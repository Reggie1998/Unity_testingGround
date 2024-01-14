using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : StateManager<IdleState.IdleStateEnum>
{
    // public IdleState() : base(IdleStateEnum.REST)
    // {

    // }
    // public IdleState(IdleStateEnum key) : base(key)
    // {

    // }

    public enum IdleStateEnum
    {
        WALK,
        REST,
    }

    void Awake()
    {
        //CurrentState = States[IdleStateEnum.REST];
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    //implement abstract methods


    // Update is called once per frame
    void Update()
    {

    }
}
