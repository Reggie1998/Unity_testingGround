using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.AI;

public struct EnemyStateContext
{
    public EnemyStateContext(NavMeshAgent agent, EnemyState parentObj)
    {
        navMeshAgent = agent;
        parent = parentObj;
    }
    public NavMeshAgent navMeshAgent;
    public EnemyState parent;

}

public class RayOrigin
{
    public RayOrigin(Vector3 directionV, float distance, string rayName)
    {
        name = rayName;
        direction = directionV;
        maxDistance = distance;
    }

    public Vector3 direction;
    public float maxDistance;
    public string name;
    public EnemyState parentRef;
    public Transform transform;

    public RaycastHit hit { get; set; }

    public bool hitInRange { get; set; }

    private Color color = Color.red;

    public bool DrawRay(GameObject gameObject)
    {
        Debug.DrawRay(gameObject.transform.position, direction * maxDistance, color);
        RaycastHit _hit;
        Physics.Raycast(gameObject.transform.position, direction, out _hit, maxDistance, layerMask: 1 << 7);
        hit = _hit;
        if (_hit.collider != null) {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            //get distance from hit point to gameObject
            var distance = Vector3.Distance(gameObject.transform.position, hit.point);
            var isPerpendicular = name.Contains("perpendicular");
            Debug.Log(Mathf.Sqrt(1.5f * 1.5f + 1.5f * 1.5f));
            var maxDistance = isPerpendicular ? Mathf.Sqrt(1.5f * 1.5f + 1.5f * 1.5f) : 1.5f;
            if (distance < maxDistance) {
                color = Color.green;
                Debug.Log("Hit: " + name);
                hitInRange = true;
                return true;
            } else {
                color = Color.red;
                hitInRange = false;
                return false;
            }
        } else {
            hitInRange = false;
            color = Color.red;
            return false;
        }
    }
}

public class EnemyState : StateManager<EnemyState.EnemyStateOptions>
{
    public EnemyStateContext context;

    public Dictionary<string, RayOrigin> raycastHits = new Dictionary<string, RayOrigin>();

    public bool hasAtleastOneCollision = false;

    public enum EnemyStateOptions
    {
        IDLING,
        WALKING
    }
    // Start is called before the first frame update
    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
        context = new EnemyStateContext(agent, this);
        // availableStates.Add(EnemyStateOptions.IDLING, EnemyStateOptions.WALKING);
        // availableStates.Add(EnemyStateOptions.WALKING, EnemyStateOptions.IDLING);
        States.Add(EnemyStateOptions.IDLING, new IdlingState(EnemyStateOptions.IDLING, context));
        States.Add(EnemyStateOptions.WALKING, new WalkingState(EnemyStateOptions.WALKING));
        CurrentState = States[EnemyStateOptions.IDLING];
        CurrentState.EnterState();
        float distance = 3.0f;
        raycastHits.Add("forward", new RayOrigin(transform.forward, distance, "forward"));
        raycastHits.Add("right", new RayOrigin(transform.right, distance, "right"));
        raycastHits.Add("up", new RayOrigin(transform.up, distance, "up"));
        raycastHits.Add("left", new RayOrigin(-transform.right, distance, "left"));
        raycastHits.Add("down", new RayOrigin(-transform.up, distance, "down"));
        raycastHits.Add("back", new RayOrigin(-transform.forward, distance, "back"));
        raycastHits.Add("perpendicular", new RayOrigin(transform.forward + transform.right, distance, "perpendicular"));
        //raycastHits.Add("perpendicularDown", new RayOrigin(transform.forward + transform.right + -transform.up, distance));
        //raycastHits.Add("perpendicularDownRight", new RayOrigin(transform.forward + transform.right + -transform.up + transform.right, distance));
        //raycastHits.Add("perpendicularDownLeft", new RayOrigin(transform.forward + transform.right + -transform.up + -transform.right, distance));
        raycastHits.Add("perpendicularUp", new RayOrigin(transform.forward + transform.up, distance, "perpendicularUp"));
        //PERPENDICULARS ALL PERHAPS SHOULD BE SMALLER BY 0.5f
        raycastHits.Add("perpendicularDown", new RayOrigin(transform.forward + -transform.up, distance, "perpendicularDown"));
        raycastHits.Add("perpendicularUpRight", new RayOrigin(transform.forward + transform.up + transform.right, distance, "perpendicularUpRight"));
        raycastHits.Add("perpendicularUpLeft", new RayOrigin(transform.forward + transform.up + -transform.right, distance, "perpendicularUpLeft"));
        raycastHits.Add("perpendicularLeft", new RayOrigin(transform.forward + -transform.right, distance, "perpendicularLeft"));
    }

    public void DrawRays()
    {
        bool atleastOneCollision = false;
        foreach (KeyValuePair<string, RayOrigin> entry in raycastHits) {
            if (atleastOneCollision == true) {
                entry.Value.DrawRay(gameObject);
            } else {
                atleastOneCollision = entry.Value.DrawRay(gameObject);
            }
        }
        hasAtleastOneCollision = atleastOneCollision;
    }

    // Update is called once per frame
    void Update()
    {
        //DrawRays();
    }

    void FixedUpdate()
    {
        DrawRays();
    }

}

public class IdlingState : BaseState<EnemyState.EnemyStateOptions>
{
    EnemyStateContext context;
    private float currentWaitTime = 0;
    public IdlingState(EnemyState.EnemyStateOptions key, EnemyStateContext context) : base(key)
    {
        this.context = context;
    }

    public override void UpdateState()
    {
        currentWaitTime += Time.deltaTime;
        Debug.Log(currentWaitTime);
        if (currentWaitTime > 5) {
            currentWaitTime = 0;
            context.parent.TransitionToState(EnemyState.EnemyStateOptions.WALKING);
            Debug.Log("Transition to Walking State");
            //TransitionToState(EnemyState.EnemyStateOptions.WALKING);
        }
    }

    public override void EnterState()
    {
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

public class WalkingState : BaseState<EnemyState.EnemyStateOptions>
{
    public WalkingState(EnemyState.EnemyStateOptions key) : base(key)
    {

    }

    public override void EnterState()
    {
    }

    public override void ExistState()
    {
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
    }
}
