using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    private Vector3 targetLocation;
    private NavMeshAgent agent;

    [SerializeField]
    public GameObject LookAtThis;

    public GameObject innner;

    private RaycastHit hit;
    private NavMeshHit hit2;

    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;

            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            {
                targetLocation = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                agent.speed = 20.0f;
                agent.destination = targetLocation;
            }
        }

        if (LookAtThis) {
            Vector3 targetPostition = new Vector3(LookAtThis.transform.position.x,
                                                this.transform.position.y,
                                                LookAtThis.transform.position.z);
            var targetRotation = Quaternion.LookRotation(targetPostition - transform.position);
            //transform.SetPositionAndRotation(transform.position, Quaternion.Slerp(transform.rotation, targetRotation, 3.0f * Time.deltaTime));
            transform.SetLocalPositionAndRotation(transform.position, Quaternion.Slerp(transform.rotation, targetRotation, 3.0f * Time.deltaTime));


        }

        if (Input.GetMouseButtonDown(0)) {
            moveBack();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            moveBack(KeyCode.L);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            moveBack(KeyCode.R);
        }

    }

    void moveBack(KeyCode side = KeyCode.None)
    {
        if (KeyCode.R == side) {
            pos = gameObject.transform.position - gameObject.transform.right * Random.Range(3.5f, 6f);
            agent.destination = pos;
            return;
        } else if (KeyCode.L == side) {
            pos = gameObject.transform.position - (-gameObject.transform.right) * Random.Range(3.5f, 6f);
            agent.destination = pos;
            return;
        }

        //agent.Move()
        pos = gameObject.transform.position - gameObject.transform.forward * Random.Range(3.5f, 6f);
        agent.destination = pos;

    }

    void OnDrawGizmos()
    {
        // Gizmos.DrawSphere(pos, 2.0f);
        // Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position - gameObject.transform.forward * Random.Range(1.0f, 3.0f));
        // Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position - gameObject.transform.right * Random.Range(1.0f, 3.0f));

    }
}
