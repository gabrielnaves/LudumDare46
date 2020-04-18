/** MinMaxRangeAttribute.cs
 * Original version by Eddie Cameron – For the public domain
 * Additional features by Gabriel Naves
 * ———————————
 * Use a MinMaxRange class to replace twin float range values
 * (eg: float minSpeed, maxSpeed; becomes MinMaxRange speed)
 *
 * Apply a [MinMaxRange( minLimit, maxLimit )] attribute to a MinMaxRange
 * instance to control the limits and to show a slider in the inspector
 */

using UnityEngine;

public class MinMaxAttribute : PropertyAttribute {
    public float minLimit, maxLimit;

    public MinMaxAttribute(float minLimit, float maxLimit) {
        this.minLimit = minLimit;
        this.maxLimit = maxLimit;
    }
}

[System.Serializable]
public class MinMax {

    public float min, max;

    public MinMax(float min, float max) {
        this.min = min;
        this.max = max;
    }

    public float Random() {
        return UnityEngine.Random.Range(min, max);
    }

    public float Range() {
        return max - min;
    }

    public float MidPoint() {
        return (min + max) / 2f;
    }

    public float Clamp(float value) {
        return Mathf.Clamp(value, min, max);
    }

    public float Lerp(float percentage) {
        return Mathf.Lerp(min, max, percentage);
    }

    public float ReverseLerp(float percentage) {
        return Mathf.Lerp(max, min, percentage);
    }

    static public MinMax Lerp(MinMax a, MinMax b, float t) {
        return new MinMax(Mathf.Lerp(a.min, b.min, t), Mathf.Lerp(a.max, b.max, t));
    }

    public float PercentageOfValueInRange(float value) {
        return (Clamp(value) - min) / (Range());
    }

    public void Scale(float scaling) {
        min *= scaling;
        max *= scaling;
    }

    public MinMax Scaled(float scaling) {
        return new MinMax(min * scaling, max * scaling);
    }

    public void Add(MinMax b) {
        min += b.min;
        max += b.max;
    }

    public MinMax Add(float value) {
        return new MinMax(min + value, max + value);
    }

    public bool IsValueInside(float value) {
        return value > min && value < max;
    }
}
