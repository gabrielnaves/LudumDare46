using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float gravityScale = 3;
    public float acceleration;
    public float maxSpeed;
    public float groundcheckDistance = 0.1f;
    public LayerMask floorMask;

    [ViewOnly] public float speed;
    [ViewOnly] public Vector2 movementDirection;
    [ViewOnly] public bool isGrounded;

    Rigidbody body;
    CapsuleCollider col;

    void Awake() {
        body = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update() {
        Move();
        AlignToWorld();
        CheckGround();
        ApplyGravity();
    }

    void Move() {
        if (GameInput.instance) {
            if (GameInput.instance.isMoving) {
                movementDirection = GameInput.instance.movement;
                speed += acceleration * Time.deltaTime;
            }
            else
                speed -= acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, maxSpeed);

            transform.position += transform.right * movementDirection.x * speed * Time.deltaTime;
            transform.position += transform.forward * movementDirection.y * speed * Time.deltaTime;
        }
    }
    
    void AlignToWorld() {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, floorMask)) {
            // Code for interpolated normal taken from here:
            // https://docs.unity3d.com/ScriptReference/RaycastHit-barycentricCoordinate.html

            // Just in case, also make sure the collider also has a renderer
            // material and texture
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null) {
                return;
            }

            Mesh mesh = meshCollider.sharedMesh;
            Vector3[] normals = mesh.normals;
            int[] triangles = mesh.triangles;

            // Extract local space normals of the triangle we hit
            Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
            Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
            Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];

            // interpolate using the barycentric coordinate of the hitpoint
            Vector3 baryCenter = hit.barycentricCoordinate;

            // Use barycentric coordinate to interpolate normal
            Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
            // normalize the interpolated normal
            interpolatedNormal = interpolatedNormal.normalized;

            // Transform local space normals to world space
            Transform hitTransform = hit.collider.transform;
            interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);

            body.MoveRotation(Quaternion.FromToRotation(Vector3.up, interpolatedNormal));
        }
    }

    void CheckGround() {
        Vector3 raycastOrigin = transform.position - transform.up * col.height / 2;
        isGrounded = Physics.Raycast(raycastOrigin, -transform.up, groundcheckDistance, floorMask);
    }

    void ApplyGravity() {
        if (!isGrounded)
            body.AddForce(-transform.up * Mathf.Abs(Physics.gravity.y) * gravityScale);
    }
}
