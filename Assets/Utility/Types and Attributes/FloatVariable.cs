using UnityEngine;

[CreateAssetMenu(menuName="Variables/FloatVariable")]
public class FloatVariable : ScriptableObject {

    public float value_;
    public bool constant;

    public float value {
        get {
            return value_;
        }
        set {
            if (!constant)
                value_ = value;
        }
    }
}
