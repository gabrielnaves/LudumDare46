using UnityEngine;

[CreateAssetMenu(menuName="Variables/ColorVariable")]
public class ColorVariable : ScriptableObject {

    public Color value_;
    public bool constant;

    public Color value {
        get {
            return value_;
        }
        set {
            if (!constant)
                value_ = value;
        }
    }
}
