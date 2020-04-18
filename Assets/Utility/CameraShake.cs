using UnityEngine;

public class CameraShake : MonoBehaviour {

    static public CameraShake instance { get; private set; }

    public bool shakeOnUpdate;
    public float traumaDecreaseRate = .75f;
    public float maxAmplitude = 1;
    public float perlinSpeed = 1;
    
    [ViewOnly] public float trauma;
    [ViewOnly] public bool constant;

    float perlinY1;
    float perlinY2;

    public void AddTrauma(float value) {
        trauma = Mathf.Clamp01(trauma + value);
    }

    public void SetConstantTrauma(float value) {
        constant = true;
        trauma = value;
    }

    public void StopConstantTrauma() {
        constant = false;
    }

    void Awake() {
        instance = (CameraShake)Singleton.Setup(this, instance);
        trauma = 0;
        perlinY1 = Random.Range(-1000, 1000);
        perlinY2 = Random.Range(-1000, 1000);
    }

    void Update() {
        UpdateTrauma();
        if (shakeOnUpdate) {
            var offset = GetShakeOffset();
            transform.position = new Vector3(offset.x, offset.y, transform.position.z);
        }
    }

    void UpdateTrauma() {
        if (!constant)
            trauma = Mathf.Clamp01(trauma - Time.deltaTime * traumaDecreaseRate);
    }

    public Vector2 GetShakeOffset() {
        float perlinPos = Time.time * perlinSpeed;
        float x = Mathf.Clamp01(Mathf.PerlinNoise(perlinY1, perlinPos));
        float y = Mathf.Clamp01(Mathf.PerlinNoise(perlinY2, perlinPos));
        Vector2 rand = new Vector2(x, y) * 2 - Vector2.one;
        return rand * trauma * trauma * maxAmplitude;
    }
}
