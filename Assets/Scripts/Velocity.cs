using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour {
    
    private Rigidbody rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.velocity = new Vector3(-33.0f, 0.0f, 0.0f);
    }

}
