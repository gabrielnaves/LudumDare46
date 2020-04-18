using System;
using UnityEngine;

public class Pair<A, B> {

    public A first;
    public B second;

    public Pair() {}

    public Pair(A a, B b) {
        first = a;
        second = b;
    }
}

[Serializable]
public class PairGameObjectInt : Pair<GameObject, int> {

    public PairGameObjectInt() {}

    public PairGameObjectInt(GameObject obj, int i) {
        first = obj;
        second = i;
    }
}
