using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [Range(0,2)]
    public float collisionOverlapSphere = 1.0f;

    private Rigidbody rigidbody;
    private Vector3 velocityBeforePhysicsUpdate; 

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
    } 
    
    void FixedUpdate() {
        velocityBeforePhysicsUpdate = rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.DrawLine(collision.contacts[0].point, transform.position, Color.red, 1.0f, false);
        // Debug.Log(collision.impulse);

        Vector3 bouceDirection = (collision.contacts[0].point - transform.position).normalized * -1;

        if (collision.gameObject.CompareTag("Bouncer")) {
            rigidbody.velocity = bouceDirection * velocityBeforePhysicsUpdate.magnitude;
        }

        // Collision force 3000 will enable physics in range of 1.0f
        Vector3 collisionForce = collision.impulse * Time.deltaTime;

        Collider[] colliders = Physics.OverlapSphere(transform.position, collisionOverlapSphere);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].CompareTag("Brick")) {
                colliders[i].GetComponent<Rigidbody>().isKinematic = false;
                // Debug.DrawLine(colliders[i].transform.position, transform.position, Color.red, 1.0f, false);
            }
        }

        collision.rigidbody.AddForce(collisionForce);
    }
}
