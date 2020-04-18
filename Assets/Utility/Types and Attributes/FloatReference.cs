using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloatReference {

    public bool useConstant = true;
    public float constantValue;
    public FloatVariable variable;

    public float value {
        get {
            return useConstant ? constantValue : variable.value;
        }
        set {
            if (!useConstant)
                variable.value = value;
        }
    }

    public static float operator /(FloatReference a, float b) {
        return a.value / b;
    }

    public static float operator /(FloatReference a, FloatReference b) {
        return a.value / b.value;
    }

    public static float operator *(FloatReference a, float b) {
        return a.value * b;
    }

    public static float operator *(FloatReference a, FloatReference b) {
        return a.value * b.value;
    }

    public static FloatReference operator +(FloatReference a, float b) {
        FloatReference result = new FloatReference() {
            useConstant = a.useConstant,
            constantValue = a.constantValue,
            variable = a.variable
        };
        result.value += b;
        return result;
    }
}
