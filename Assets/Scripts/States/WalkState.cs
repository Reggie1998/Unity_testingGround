using UnityEngine;

public class WalkState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;

    Vector3 destinationTarget;

    private float currentWalktime = 0;
    public WalkState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Walking State");
        findDestination();
        context.parent.WalkTowards(destinationTarget);

    }

    public void findDestination()
    {
        Physics.CheckSphere(context.parent.gameObject.transform.position, 4.0f);
        destinationTarget = Random.insideUnitSphere * 10;
    }

    public override void ExistState()
    {
        Debug.Log("Exit Walking State");
        //Stop walking idk if this is a good method to do it
        context.parent.WalkTowards(context.parent.gameObject.transform.position);
        destinationTarget = Vector3.zero;
    }


    //why do we need this?
    public override EnemyState.EnemyStateOptions GetNextState()
    {
        return EnemyState.EnemyStateOptions.WALKING;
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
        currentWalktime += Time.deltaTime;
        if (context.parent.IsEnemyInSight()) {
            Debug.Log("Transition to Chasing State");
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.CHASING);
        }
        if (currentWalktime > 2) {
            currentWalktime = 0;
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.IDLING);
            Debug.Log("Transition to Idling State");
        } else if (Vector3.Distance(context.parent.gameObject.transform.position, destinationTarget) < 1.0f) {
            findDestination();
            context.parent.WalkTowards(destinationTarget);
        }
    }

    void DrarwGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(destinationTarget, 1.0f);
    }
}