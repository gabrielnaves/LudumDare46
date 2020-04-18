using System;
using System.Collections.Generic;

static public class ListExtensions {

    /// From https://stackoverflow.com/questions/273313/randomize-a-listt
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool IsEqualTo<T>(this IList<T> list, IList<T> other) {
        if (list.Count != other.Count)
            return false;
        for (int i = 0; i < list.Count; ++i) {
            if (!list[i].Equals(other[i]))
                return false;
        }
        return true;
    }
}
