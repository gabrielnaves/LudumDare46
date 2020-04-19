using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float height;
    public float translationSmoothTime;
    public LayerMask floorMask;
    public Vector3 angleOffset = new Vector3(90, 0, 0);

    Vector3 translationSpeed;

    void LateUpdate() {
        if (target)
            transform.position = Vector3.SmoothDamp(transform.position, target.position + target.up * height, ref translationSpeed, translationSmoothTime);
        LookAtTarget();
    }

    void LookAtTarget() {
        transform.LookAt(target.position, target.forward);
        //transform.rotation = Quaternion.Euler(90 + target.rotation.eulerAngles.x, 0, 0);
    }
}
