using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Rigidbody rigidbody;
    private Vector3 velocityBeforePhysicsUpdate; 

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
    } 
    
    void FixedUpdate() {
        velocityBeforePhysicsUpdate = rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        // Debug.DrawLine(collision.contacts[0].point, transform.position, Color.red, 1.0f, false);
        // Debug.Log(collision.impulse);
        
        Vector3 bouceDirection = (collision.contacts[0].point - transform.position).normalized * -1;

        if (collision.gameObject.CompareTag("Bouncer")) {
            rigidbody.velocity = bouceDirection * velocityBeforePhysicsUpdate.magnitude;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].CompareTag("Brick")) {
                colliders[i].GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
