using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    public GameObject LookAtThis;

    [SerializeField]
    public bool chase = false;

    private NavMeshAgent agent;

    private EnemyState enemyState;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        if (agent != null) {
            agent.updateRotation = false;
        }
        enemyState = GetComponent<EnemyState>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (chase) {
            if (agent != null && agent.isActiveAndEnabled == true) {
                agent.speed = 10.0f;
                agent.destination = LookAtThis.transform.position;
            }
        }
        var forwardHit = enemyState.raycastHits["forward"];
        var downHit = enemyState.raycastHits["down"];
        var downPHit = enemyState.raycastHits["perpendicularDown"];
        var upPHit = enemyState.raycastHits["perpendicularUp"];
        if (forwardHit.hitInRange == true || downPHit.hitInRange == true || downHit.hitInRange == true) {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            chase = false;
            if (agent.isOnNavMesh == true && agent.isStopped == false) {
                agent.isStopped = true;
                agent.enabled = false;
                agent.updatePosition = false;
            }
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemyState.raycastHits["forward"].hit.point, 0.1f);

            //if upPHit is not null, move upward
            Debug.Log("ForwardHit.hitInRange: " + forwardHit.hitInRange);
            if (forwardHit.hitInRange == true || (downPHit.hitInRange == true && forwardHit.hitInRange == true)) {
                Debug.Log("forward is not null and downPhit is not null");
                rb.useGravity = false;
                var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
            } else {
                rb.useGravity = true;
            }

            //if forward is null and downPhit is not null, move forward
            if (forwardHit.hitInRange == false && downPHit.hitInRange == true) {
                Debug.Log("forward is null and downPhit is not null");
                var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.05f);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
                //shouldMoveForward = true;
            }

            if (forwardHit.hitInRange == false && downPHit.hitInRange == false && downHit.hitInRange == true) {
                Debug.Log("Launching");
                //add force upwards at 45 degree angle
                rb.AddForce((transform.forward + transform.up) * 60);
            }
            // //if forward is not null and downPhit is not null, move upward
            // if (forwardHit.collider != null && downPHit.collider != null) {
            //     var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            //     gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
            // }

            // if (forwardHit.collider == null) {
            //     var target = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
            //     gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
            //     shouldMoveForward = true;
            // }
            // if (shouldMoveForward) {
            //     var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            //     gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
            // } else {
            //     var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            //     gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
            // }
        }
    }

    public void LateUpdate()
    {
        if (LookAtThis) {
            Vector3 targetPostition = new Vector3(LookAtThis.transform.position.x,
                                                this.transform.position.y,
                                                LookAtThis.transform.position.z);
            var targetRotation = Quaternion.LookRotation(targetPostition - transform.position);
            //transform.SetPositionAndRotation(transform.position, Quaternion.Slerp(transform.rotation, targetRotation, 3.0f * Time.deltaTime));
            transform.SetLocalPositionAndRotation(transform.position, Quaternion.Slerp(transform.rotation, targetRotation, 3.0f * Time.deltaTime));

        }
    }
}
