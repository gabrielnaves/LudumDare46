using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float height;

    void LateUpdate() {
        if (target) {
            transform.position = target.position + target.up * height;
            Debug.Log(target.rotation.eulerAngles);
        }
    }
}
