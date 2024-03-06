using System.Linq.Expressions;
using System.Net.WebSockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

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

        // if (chase) {
        //     if (agent != null && agent.isActiveAndEnabled == true) {
        //         agent.speed = 10.0f;
        //         //agent.destination = LookAtThis.transform.position;
        //     }
        // }
        //Debug.Log(gameObject.transform.position);
        var forwardHit = enemyState.raycastHits["forward"];
        var downHit = enemyState.raycastHits["down"];
        //downHit.hit.
        var downPHit = enemyState.raycastHits["perpendicularDown"];
        var upPHit = enemyState.raycastHits["perpendicularUp"];
        if (forwardHit.hitInRange == true) {
            rb.useGravity = false;
            if (agent.isOnNavMesh == true && agent.isStopped == false) {
                agent.isStopped = true;
                agent.enabled = false;
                agent.updatePosition = false;
            }
            var idk = Vector3.ProjectOnPlane(gameObject.transform.forward * 55, forwardHit.hit.normal).normalized;
            Debug.DrawLine(idk, idk * 2, Color.yellow);
            var a = new Vector3((gameObject.transform.position.x + idk.x), (gameObject.transform.position.y + idk.y), (gameObject.transform.position.z + idk.z)); Debug.DrawLine(idk, idk * 2, Color.yellow);
            Debug.DrawLine(forwardHit.hit.point + idk, forwardHit.hit.point + (idk * 2), Color.yellow);
            var hitAdj = (forwardHit.hit.point + idk);
            hitAdj.z -= 1f;
            var goal = forwardHit.hit.point + (idk * 2);
            var actualGoal = gameObject.transform.TransformDirection(goal);
            goal.z -= 1f;
            Debug.DrawLine(hitAdj, goal, Color.yellow);
            //rb.AddForce(goal * 2, ForceMode.Impulse);

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, actualGoal, Time.deltaTime * 2.5f);

            chase = false;
            if (agent.isOnNavMesh == true && agent.isStopped == false) {
                // agent.isStopped = true;
                // agent.enabled = false;
                // agent.updatePosition = false;
            }
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemyState.raycastHits["forward"].hit.point, 0.1f);

            //if upPHit is not null, move upward
            Debug.Log("ForwardHit.hitInRange: " + forwardHit.hitInRange);
            if (forwardHit.hitInRange == true || (downPHit.hitInRange == true && forwardHit.hitInRange == true)) {
                //rb.useGravity = false;
                var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z);
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
            } else {
                //rb.useGravity = true;
            }

            //if forward is null and downPhit is not null, move forward
            if (forwardHit.hitInRange == false && downPHit.hitInRange == true) {
                var target = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.05f);
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 0.3f);
                //shouldMoveForward = true;
            }



            if (forwardHit.hitInRange == false && downPHit.hitInRange == false && downHit.hitInRange == true) {
                //add force upwards at 45 degree angle
                //rb.AddForce((transform.forward + transform.up) * 60, ForceMode.Impulse);
                //rb.velocity *= 2;
            }

        } else {
            if (rb.useGravity == false) {
                rb.useGravity = true;
            }
        }
    }

}
