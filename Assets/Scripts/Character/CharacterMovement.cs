using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float gravityScale = 3;
    public float acceleration;
    public float maxSpeed;
    public LayerMask floorMask;

    [ViewOnly] public float speed;

    Vector2 movementDirection;
    Rigidbody body;

    void Awake() {
        body = GetComponent<Rigidbody>();
    }

    void Update() {
        Move();
        AlignToWorld();
    }

    void Move() {
        if (GameInput.instance) {
            Vector2 movement = GameInput.instance.movement;
            if (movement != Vector2.zero) {
                movementDirection = movement;
                speed += acceleration * Time.deltaTime;
            }
            else
                speed -= acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, maxSpeed);

            transform.position += transform.right * movementDirection.x * speed * Time.deltaTime;
            transform.position += transform.forward * movementDirection.y * speed * Time.deltaTime;

            //body.AddForce(transform.right * movement.x * speed);
            //body.AddForce(transform.forward * movement.y * speed);
        }
    }
    
    void AlignToWorld() {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, floorMask)) {
            body.MoveRotation(Quaternion.FromToRotation(Vector3.up, hit.normal));
            body.AddForce(-transform.up * Mathf.Abs(Physics.gravity.y) * gravityScale);
        }
    }
}
