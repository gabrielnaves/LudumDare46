using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Variables/IntVariable")]
public class IntVariable : ScriptableObject {

    public int value_;
    public bool constant;

    public int value {
        get {
            return value_;
        }
        set {
            if (!constant)
                value_ = value;
        }
    }
}
