using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntReference {

    public bool useConstant = true;
    public int constantValue;
    public IntVariable variable;

    public int value {
        get {
            return useConstant ? constantValue : variable.value;
        }
        set {
            if (!useConstant)
                variable.value = value;
        }
    }
}
