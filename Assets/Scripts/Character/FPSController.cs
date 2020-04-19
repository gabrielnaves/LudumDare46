using UnityEngine;

public class FPSController : MonoBehaviour {

    public MovementSettings movementSettings;
    public LookSettings lookSettings;

    public Transform fpsCamera;

    [ViewOnly] public bool grounded;

    CapsuleCollider col;

    float speed;
    Vector2 movementDirection;
    float camXRotation;
    Vector3 targetPosition;

    void Awake() {
        col = GetComponent<CapsuleCollider>();
    }

    void Update() {
        UpdateCamera();
        CalculateMovement();
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

        Vector3 movement = transform.right * movementDirection.x * speed * Time.deltaTime + transform.forward * movementDirection.y * speed * Time.deltaTime;
        targetPosition = transform.position + movement;
    }

    void Groundcheck() {
        Vector3 origin = targetPosition + transform.up * col.radius * 2;
        float maxDistance = col.radius * 2 + movementSettings.groundcheckDistance;
        if (Physics.SphereCast(origin, col.radius, -transform.up, out RaycastHit hit, maxDistance, movementSettings.groundLayer)) {
            grounded = true;
            targetPosition = hit.point;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            Debug.Log(transform.up);
        }
        else
            grounded = false;
    }

    void ApplyGravityIfFalling() {
        if (grounded) return;
        Vector3 gravityDisplacement = -transform.up * movementSettings.gravityScale * Mathf.Abs(Physics.gravity.y) * Time.deltaTime;
        if (Physics.SphereCast(targetPosition, col.radius, gravityDisplacement, out RaycastHit hit, gravityDisplacement.magnitude, movementSettings.groundLayer))
            targetPosition = hit.point;
        else
            targetPosition += gravityDisplacement;
    }

    //Vector3 InterpolateNormal(RaycastHit hit) {
    //    // Code for interpolated normal taken from here:
    //    // https://docs.unity3d.com/ScriptReference/RaycastHit-barycentricCoordinate.html

    //    // Just in case, also make sure the collider also has a renderer
    //    // material and texture
    //    MeshCollider meshCollider = hit.collider as MeshCollider;
    //    if (meshCollider == null || meshCollider.sharedMesh == null) {
    //        return Vector3.up;
    //    }

    //    Mesh mesh = meshCollider.sharedMesh;
    //    Vector3[] normals = mesh.normals;
    //    int[] triangles = mesh.triangles;

    //    // Extract local space normals of the triangle we hit
    //    Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
    //    Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
    //    Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];

    //    // interpolate using the barycentric coordinate of the hitpoint
    //    Vector3 baryCenter = hit.barycentricCoordinate;

    //    // Use barycentric coordinate to interpolate normal
    //    Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
    //    // normalize the interpolated normal
    //    interpolatedNormal = interpolatedNormal.normalized;

    //    // Transform local space normals to world space
    //    Transform hitTransform = hit.collider.transform;
    //    interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);
    //    return interpolatedNormal;
    //}

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
