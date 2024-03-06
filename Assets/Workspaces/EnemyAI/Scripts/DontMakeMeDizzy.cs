using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMakeMeDizzy : MonoBehaviour
{
    [SerializeField]
    public GameObject followThis;
    [SerializeField]
    public Vector3 cameraDistance;

    [SerializeField]
    public Vector3 cameraRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.rotation;
        transform.position = new Vector3(
            followThis.transform.position.x + cameraDistance.x,
            followThis.transform.position.y + cameraDistance.y,
            followThis.transform.position.z + cameraDistance.z
        );
        transform.rotation = new Quaternion(
            transform.rotation.x + cameraRotation.x,
            transform.rotation.y + cameraRotation.y,
            transform.rotation.z + cameraRotation.z,
            1.0f
        );
    }
}
