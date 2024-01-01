using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMakeMeDizzy : MonoBehaviour
{
    [SerializeField]
    public GameObject followThis;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.rotation;
        transform.position = new Vector3(
            followThis.transform.position.x,
            followThis.transform.position.y + 15.0f,
            followThis.transform.position.z - 5.0f
        );
    }
}
