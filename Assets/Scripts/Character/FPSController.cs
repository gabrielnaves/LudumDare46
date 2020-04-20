using UnityEngine;

public class FPSController : MonoBehaviour {

    public MovementSettings movementSettings;
    public LookSettings lookSettings;

    public Transform fpsCamera;
    public LayerMask collisionmask;
    [Range(0, 1)]

    [ViewOnly] public bool grounded;

    Rigidbody body;
    SphereCollider col;

    Vector2 movementDirection;
    float camXRotation;
    Vector3 desiredMovement;
    Vector3 groundNormal = Vector3.up;
    Vector3 normalSpeed;

    void Awake() {
        body = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    void Update() {
        UpdateCamera();
    }

    void FixedUpdate() {
        CalculateMovement();
        //CalculateCollisions();
        Groundcheck();
        UpdateDrag();
        AlignToGround();
        ApplyGravity();
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
            desiredMovement = transform.right * movementDirection.x + transform.forward * movementDirection.y;
            desiredMovement *= movementSettings.maxSpeed;
            if (body.velocity.sqrMagnitude < movementSettings.maxSpeed * movementSettings.maxSpeed)
                body.AddForce(desiredMovement, ForceMode.Impulse);
        }
    }

    void Groundcheck() {
        Vector3 origin = transform.position + transform.up * col.radius;
        float maxDistance = col.radius * 2 + movementSettings.groundcheckDistance;
        grounded = Physics.Raycast(origin, -transform.up, maxDistance, movementSettings.groundLayer);
    }

    void UpdateDrag() {
        body.drag = grounded ? 5 : 0;
    }

    void AlignToGround() {
        Vector3 origin = transform.position + transform.up * col.radius;
        if (Physics.Raycast(origin, -transform.up, out RaycastHit hit, Mathf.Infinity, movementSettings.groundLayer)) {
            Vector3 hitNormal = InterpolateNormal(hit);
            if (grounded)
                body.velocity = Vector3.ProjectOnPlane(body.velocity, hitNormal);
            groundNormal = Vector3.SmoothDamp(groundNormal, hitNormal, ref normalSpeed, grounded ? 0.1f : 0.5f);
            transform.rotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        }
    }

    void ApplyGravity() {
        if (!grounded)
            body.AddForce(-transform.up * Mathf.Abs(Physics.gravity.y) * movementSettings.gravityScale, ForceMode.Acceleration);
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
    }

    [System.Serializable]
    public class LookSettings {

        public float minX = -80f;
        public float maxX = 90;
    }
}
