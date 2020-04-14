using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour {
    
    public Vector3 velocityChange = new Vector3(15f, 0.0f, 0.0f);

    private Rigidbody rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddRelativeForce(velocityChange, ForceMode.VelocityChange);
    }
}
