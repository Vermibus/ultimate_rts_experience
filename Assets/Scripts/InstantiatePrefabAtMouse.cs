using System.Collections.Generic;
using System.Collections;
using UnityEngine;
    
public class InstantiatePrefabAtMouse : MonoBehaviour {
    public GameObject gameObject;
    public GameObject cameraParent;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            GameObject ball = Instantiate(gameObject, transform.position, Quaternion.Euler(transform.worldToLocalMatrix.MultiplyVector(transform.forward)));
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.AddRelativeForce(transform.forward * 35.0f, ForceMode.VelocityChange);
        }
    }

}
