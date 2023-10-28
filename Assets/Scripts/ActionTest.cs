using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour {

    Action v;

    [ContextMenu("Test")]
    private void Test() {

        Subscribe(ref v);
        v?.Invoke();
    }

    private void Subscribe(ref Action a) => a += Print;
    private void Print() => Debug.Log("Hello");


}
