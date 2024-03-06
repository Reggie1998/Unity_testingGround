using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public struct EnemyStateContext
{
    //Isn't this worthless just pass the parent and having anything needed as public?
    public EnemyStateContext(EnemyState parentObj)
    {
        parent = parentObj;
    }
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
        Debug.DrawRay(gameObject.transform.position, (transform.forward + direction) * maxDistance, color);
        RaycastHit _hit;
        Physics.Raycast(gameObject.transform.position, transform.forward + direction, out _hit, maxDistance, layerMask: 1 << 7);
        hit = _hit;
        if (_hit.collider != null) {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            //get distance from hit point to gameObject
            var distance = Vector3.Distance(gameObject.transform.position, hit.point);
            var isPerpendicular = name.Contains("perpendicular");
            //Debug.Log(Mathf.Sqrt(1.5f * 1.5f + 1.5f * 1.5f));
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
    [ReadOnlyInspector]
    public EnemyStateContext context;

    [ReadOnlyInspector]
    public Vector3 centeredPosition;

    [ReadOnlyInspector]
    public GameObject currentTarget;

    [ReadOnlyInspector]
    public BoxCollider boxCollider;

    [ReadOnlyInspector]
    public Vector3 initialPosition;

    [ReadOnlyInspector]
    private string currentStateName;

    [ReadOnlyInspector]
    public float distanceeFromInitialPosition;

    [ReadOnlyInspector]
    public float distanceFromTarget;

    public bool hasAtleastOneCollision = false;

    [SerializeField]
    private float visionDistance;

    [SerializeField]
    private LayerMask aggroLayerMasks;

    public NavMeshAgent navMeshAgent;

    public Dictionary<string, RayOrigin> raycastHits = new Dictionary<string, RayOrigin>();

    private Dictionary<string, Vector3> raysDictionary = new Dictionary<string, Vector3>();

    public float rayLength = 5f;
    public int numberOfRays = 5;


    public float chaseDistance;

    public float tempAttackRange = 2f;

    public enum EnemyStateOptions
    {
        IDLING,
        WALKING,
        CHASING,
        CLIMBING,
        INAACTION,
        RECALLING,
        REPOSITIONING,

    }
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        context = new EnemyStateContext(this);
        boxCollider = GetComponent<BoxCollider>();
        centeredPosition = boxCollider.bounds.center;
        // availableStates.Add(EnemyStateOptions.IDLING, EnemyStateOptions.WALKING);
        // availableStates.Add(EnemyStateOptions.WALKING, EnemyStateOptions.IDLING);
        States.Add(EnemyStateOptions.IDLING, new IdleState(EnemyStateOptions.IDLING, context));
        States.Add(EnemyStateOptions.WALKING, new WalkState(EnemyStateOptions.WALKING, context));
        States.Add(EnemyStateOptions.CHASING, new ChaseState(EnemyStateOptions.CHASING, context));
        States.Add(EnemyStateOptions.RECALLING, new RecallState(EnemyStateOptions.RECALLING, context));
        States.Add(EnemyStateOptions.INAACTION, new InActionState(EnemyStateOptions.INAACTION, context));
        //ADD RESETING STATE
        // CurrentState = States[EnemyStateOptions.IDLING];
        // CurrentState.EnterState();
        initialPosition = transform.position;
        AddRays();
        GenerateRays();
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



    void FixedUpdate()
    {
        // // Assuming your object has a Transform component attached
        // Transform myTransform = transform;

        // // Get the local forward direction
        // Vector3 localForward = Vector3.forward;

        // // Convert the local direction to a world direction
        // Vector3 worldForward = myTransform.TransformDirection(localForward);

        // // Draw a ray from the object's position along the forward direction
        // Debug.DrawRay(myTransform.position, worldForward * 2.5f, Color.red);

        DrawRays2(gameObject.transform.rotation.eulerAngles);
    }

    void LateUpdate()
    {
        //Add attacking state to staring ? 
        if (currentTarget != null && (currentStateName == "CHASING")) {
            //transform.LookAt(transform.position, transform.up);

        }
    }

    public bool IsEnemyInSight()
    {
        //IDK IF MAX AMOUNT IS NEEDED
        int maxAmount = 100;
        Collider[] hitCollider = new Collider[maxAmount];
        int numColliders = Physics.OverlapSphereNonAlloc(centeredPosition, visionDistance, hitCollider, aggroLayerMasks);
        Collider closestCollider = null;
        var closestDistance = visionDistance;
        for (int i = 0; i < maxAmount; i++) {
            Debug.Log("HitCollider: " + hitCollider[i]);
            //get closest collider in the loop
            if (hitCollider[i] != null) {
                if (closestCollider == null) {
                    closestCollider = hitCollider[i];
                } else {
                    var distance = Vector3.Distance(gameObject.transform.position, hitCollider[i].gameObject.transform.position);
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestCollider = hitCollider[i];
                    }
                }
            }
        }
        if (closestCollider != null) {
            currentTarget = closestCollider.gameObject;
            Debug.Log("ClosestCollider: " + closestCollider.gameObject.name);
            return true;
        } else {
            return false;
        }
    }

    void GenerateRays()
    {
        // Assuming your object has a Transform component attached
        Transform myTransform = transform;

        // Clear the dictionary to avoid duplicates
        raysDictionary.Clear();

        // Calculate angles between rays
        float angleIncrement = 360f / numberOfRays;
        float[] angles = new float[] { 0, 45, 90, 135, 180, 225, 270, 315 };

        // Generate rays and store them in the dictionary
        for (int i = 0; i < angles.Length; i++) {
            //float angle = i * angleIncrement;

            // Calculate local forward direction based on the angle
            Quaternion rotation = Quaternion.Euler(0f, angles[i], 0f);
            Vector3 localForward = rotation * Vector3.forward;

            // Convert the local direction to a world direction
            Vector3 worldForward = myTransform.TransformDirection(localForward);

            // Add the ray to the dictionary with a descriptive key
            string rayKey = "Ray_" + angles[i].ToString("F0") + "Degrees";
            raysDictionary.Add(rayKey, worldForward * rayLength);
        }

        // Generate additional rays
        Vector3 upward45Degrees = Quaternion.Euler(45f, 0f, 0f) * Vector3.up;
        Vector3 upwardDirection = myTransform.up;
        Vector3 downwardDirection = -myTransform.up;
        Vector3 downward45Degrees = Quaternion.Euler(-45f, 0f, 0f) * downwardDirection;

        raysDictionary.Add("Ray_Upward45Degrees", upward45Degrees * rayLength);
        raysDictionary.Add("Ray_Upward", upwardDirection * rayLength);
        raysDictionary.Add("Ray_Downward", downwardDirection * rayLength);
        raysDictionary.Add("Ray_Downward45Degrees", downward45Degrees * rayLength);
    }

    void DrawRays2(Vector3 localRotation)
    {
        // Draw the rays using Debug.DrawRay
        foreach (var ray in raysDictionary) {
            // Apply local rotation to each ray
            Quaternion rotation = Quaternion.Euler(localRotation);
            Vector3 rotatedRay = rotation * ray.Value;

            RaycastHit _hit;
            Physics.Raycast(gameObject.transform.position, rotatedRay, out _hit, 2.5f, layerMask: 1 << 7);
            if (_hit.collider != null) {

            }
            Debug.DrawRay(transform.position, rotatedRay, _hit.collider ? Color.green : Color.red);

        }
    }

    public void WalkTowards(Vector3 destination)
    {
        Debug.Log("Walking towards: " + destination);
        navMeshAgent.enabled = true;
        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(destination);
    }

    void AddRays()
    {
        float distance = 3.0f;
        raycastHits.Add("forward", new RayOrigin(transform.forward, distance, "forward"));
        raycastHits.Add("right", new RayOrigin(transform.right, distance, "right"));
        raycastHits.Add("up", new RayOrigin(transform.up, distance, "up"));
        raycastHits.Add("left", new RayOrigin(-transform.right, distance, "left"));
        raycastHits.Add("down", new RayOrigin(-transform.up, distance, "down"));
        raycastHits.Add("back", new RayOrigin(-transform.forward, distance, "back"));
        raycastHits.Add("perpendicular", new RayOrigin(transform.right, distance, "perpendicular"));
        raycastHits.Add("perpendicularUp", new RayOrigin(transform.up, distance, "perpendicularUp"));
        raycastHits.Add("perpendicularDown", new RayOrigin(-transform.up, distance, "perpendicularDown"));
        raycastHits.Add("perpendicularUpRight", new RayOrigin(transform.up + transform.right, distance, "perpendicularUpRight"));
        raycastHits.Add("perpendicularUpLeft", new RayOrigin(transform.up + -transform.right, distance, "perpendicularUpLeft"));
        raycastHits.Add("perpendicularLeft", new RayOrigin(-transform.right, distance, "perpendicularLeft"));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centeredPosition, visionDistance);
    }
}

