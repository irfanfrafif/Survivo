using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableTest : MonoBehaviour
{
    private static GlobalVariableTest _instance;
    public static GlobalVariableTest Instance { get { return _instance; } }

    public DialogueTest activeDialogue;

    [SerializeField] private bool gay;
    [SerializeField] private bool lord;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public bool isGay(string node, int lineIndex)
    {
        return gay;
    }

    public bool isLord(string node, int lineIndex)
    {
        return lord;
    }

    public void SetGay(string node, int lineIndex, bool test)
    {
        gay = test;
    }

    public void SetGayTrue(string node, int lineIndex)
    {
        gay = true;
    }

    public void SetGayFalse(string node, int lineIndex)
    {
        gay = false;
    }
}
