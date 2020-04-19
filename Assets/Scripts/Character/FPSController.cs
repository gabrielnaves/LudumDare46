using UnityEngine;

public class FPSController : MonoBehaviour {

    public MovementSettings movementSettings;
    public LookSettings lookSettings;

    public Transform fpsCamera;
    public LayerMask collisionmask;
    public float elasticity;

    [ViewOnly] public bool grounded;

    Rigidbody body;
    CapsuleCollider col;

    float speed;
    Vector2 movementDirection;
    float camXRotation;
    Vector3 desiredMovement;
    Vector3 targetPosition;
    Vector3 groundNormal = Vector3.up;

    void Awake() {
        body = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update() {
        UpdateCamera();
        CalculateMovement();
        CalculateCollisions();
        Groundcheck();
        ApplyGravityIfFalling();
        transform.position = targetPosition;
    }

    void UpdateCamera() {
        Vector2 cameraMovement = GameInput.instance.cameraMovement;
        camXRotation -= cameraMovement.y;
        camXRotation = Mathf.Clamp(camXRotation, lookSettings.minX, lookSettings.maxX);
        fpsCamera.localRotation = Quaternion.Euler(camXRotation, 0, 0);
        transform.Rotate(transform.up * cameraMovement.x, Space.World);
    }

    void CalculateMovement() {
        if (GameInput.instance.isMoving) {
            movementDirection = GameInput.instance.movement;
            speed += movementSettings.acceleration * Time.deltaTime;
        }
        else
            speed -= movementSettings.deceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, movementSettings.maxSpeed);

        desiredMovement = transform.right * movementDirection.x * speed * Time.deltaTime + transform.forward * movementDirection.y * speed * Time.deltaTime;
        targetPosition = transform.position + desiredMovement;
    }
    
    void CalculateCollisions() {
        Vector3 origin = transform.position + transform.up * col.height / 2;
        if (Physics.SphereCast(origin, col.radius, desiredMovement, out RaycastHit hit, desiredMovement.magnitude, collisionmask)) {
            Vector3 hitdir = hit.point - origin;
            Vector3 movementUpToCollision = Vector3.Project(desiredMovement, hitdir);
            Vector3 hitNormal = hit.transform.TransformDirection(hit.normal);
            movementUpToCollision -= Vector3.Project(movementUpToCollision, hitNormal);
            Vector3 remainingMovement = desiredMovement - movementUpToCollision;
            remainingMovement -= Vector3.Project(remainingMovement, -hitNormal);
            desiredMovement = movementUpToCollision + remainingMovement * elasticity;
            targetPosition = transform.position + desiredMovement;
        }
    }

    void Groundcheck() {
        Vector3 origin = targetPosition + transform.up * col.height / 2;
        float maxDistance = col.radius * 2 + movementSettings.groundcheckDistance;
        if (Physics.Raycast(origin, -transform.up, out RaycastHit hit, maxDistance, movementSettings.groundLayer)) {
            grounded = true;
            targetPosition = hit.point;
            groundNormal = movementSettings.filter * groundNormal + (1 - movementSettings.filter) * InterpolateNormal(hit);
            transform.rotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        }
        else
            grounded = false;
    }

    void ApplyGravityIfFalling() {
        if (grounded) return;
        Vector3 gravityDisplacement = -transform.up * movementSettings.gravityScale * Mathf.Abs(Physics.gravity.y) * Time.deltaTime;
        if (Physics.Raycast(targetPosition, gravityDisplacement, out RaycastHit hit, gravityDisplacement.magnitude, movementSettings.groundLayer))
            targetPosition = hit.point;
        else
            targetPosition += gravityDisplacement;
    }

    Vector3 InterpolateNormal(RaycastHit hit) {
        // Code for interpolated normal taken from here:
        // https://docs.unity3d.com/ScriptReference/RaycastHit-barycentricCoordinate.html

        // Just in case, also make sure the collider also has a renderer
        // material and texture
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null) {
            return hit.normal;
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
        return interpolatedNormal;
    }

    [System.Serializable]
    public class MovementSettings {

        public float acceleration = 10f;
        public float deceleration = 20f;
        public float maxSpeed = 8f;
        public float gravityScale = 3f;
        public float groundcheckDistance = 0.2f;
        public LayerMask groundLayer;

        [Tooltip("How much the previous ground normal will affect the current one")]
        public float filter = 0.1f;
    }

    [System.Serializable]
    public class LookSettings {

        public float minX = -80f;
        public float maxX = 90;
    }
}
