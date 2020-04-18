using UnityEngine;
using UnityEngine.UI;

public class UIAlphaButton : MonoBehaviour {

    public float minAlpha = 0.1f;

    void Start() {
        GetComponent<Image>().alphaHitTestMinimumThreshold = minAlpha;
    }
}
