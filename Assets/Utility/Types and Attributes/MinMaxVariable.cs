using UnityEngine;

[CreateAssetMenu(menuName="Variables/MinMaxVariable")]
public class MinMaxVariable : ScriptableObject {

    public MinMax value_;
    public bool constant;

    public MinMax value {
        get {
            return value_;
        }
        set {
            if (!constant)
                value_ = value;
        }
    }
}
