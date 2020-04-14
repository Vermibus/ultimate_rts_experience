using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject camera;

    float speed = 0.015f;
    float zoomSpeed = 7.0f;
    float rotationSpeed = 0.1f;

    float minHeight = 4.0f; 
    float maxHeight = 30.0f;

    Vector2 p1;
    Vector2 p2;

    void Start () {

    }

    void Update () {

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = 0.03f;
            zoomSpeed = 10.0f;
        } else {
            speed = 0.015f;
            zoomSpeed = 5.0f;
        }

        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal");
        float vsp = transform.position.y * speed * Input.GetAxis("Vertical");
        float scrollSp = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        if ((transform.position.y >= maxHeight) && (scrollSp > 0)) {
            scrollSp = 0;
        } else if ((transform.position.y <= minHeight) && (scrollSp < 0)) {
            scrollSp = 0;
        }

        if ((transform.position.y + scrollSp) > maxHeight) {
            scrollSp = maxHeight - transform.position.y;
        } else if ((transform.position.y + scrollSp) < minHeight) {
            scrollSp = minHeight - transform.position.y;
        }

        Vector3 verticalMove = new Vector3 (0, scrollSp, 0);
        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;

        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        Vector3 move = verticalMove + lateralMove + forwardMove;

        transform.position += move;

        getCameraRotation(); 
    }

    void getCameraRotation() {
        if (Input.GetMouseButtonDown(2)) {
            p1 = Input.mousePosition; 
        }

        if (Input.GetMouseButton(2)) {
            p2 = Input.mousePosition;

            float dx = (p2 - p1).x * rotationSpeed;
            float dy = (p2 - p1).y * rotationSpeed;

            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0)); //Y rotation
            camera.transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0)); //X rotation
            p1 = p2;
        }
    }
}