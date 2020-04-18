using UnityEngine;

public class ScreenDimensions : MonoBehaviour {

    static public ScreenDimensions instance { get; private set; }

    public Vector2 referenceResolution;

    [ViewOnly] public float originalWidth;
    [ViewOnly] public float currentWidth;
    [ViewOnly] public float currentHeight;
    [ViewOnly] public float widthRatio;

    public Bounds dimensions => new Bounds(Vector3.zero, new Vector3(currentWidth, currentHeight, 1));

    void Awake() {
        instance = (ScreenDimensions)Singleton.Setup(this, instance);
        CalculateDimensions();
    }

    void Update() {
        CalculateDimensions();
    }

    void CalculateDimensions() {
        currentHeight = Camera.main.orthographicSize * 2;

        originalWidth = currentHeight * referenceResolution.x / referenceResolution.y;
        currentWidth = currentHeight * Screen.width / Screen.height;

        if (originalWidth != 0)
            widthRatio = currentWidth / originalWidth;
    }
}
