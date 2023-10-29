using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GlobalVariableTest : MonoBehaviour
{
    private static GlobalVariableTest _instance;
    public static GlobalVariableTest Instance { get { return _instance; } }

    public ObjectDialogue activeDialogue;
    private bool isInDialogue;
    public CinemachineVirtualCamera cam;

    public bool IsInDialogue
    {   get { return isInDialogue; } 
        set {
            cam.m_Lens.OrthographicSize = value ? 1.75f : 2.25f;
            isInDialogue = value; }
    }

    [SerializeField] private bool choice1A;
    [SerializeField] private bool choice1B;
    [SerializeField] private bool choice2A;
    [SerializeField] private bool choice2B;
    [SerializeField] private bool choice3A;
    [SerializeField] private bool choice3B;
    [SerializeField] private bool choice4A;
    [SerializeField] private bool choice4B;

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

    public void SetTrueChoice1A(string node, int lineIndex) { choice1A = true; }
    public bool CheckChoice1A(string node, int lineIndex) { return choice1A; }
    public void SetTrueChoice1B(string node, int lineIndex) { choice1B = true; }
    public bool CheckChoice1B(string node, int lineIndex) { return choice1B; }

    public void SetTrueChoice2A(string node, int lineIndex) { choice2A = true; }
    public bool CheckChoice2A(string node, int lineIndex) { return choice2A; }
    public void SetTrueChoice2B(string node, int lineIndex) { choice2B = true; }
    public bool CheckChoice2B(string node, int lineIndex) { return choice2B; }

    public void SetTrueChoice3A(string node, int lineIndex) { choice3A = true; }
    public bool CheckChoice3A(string node, int lineIndex) { return choice3A; }
    public void SetTrueChoice3B(string node, int lineIndex) { choice3B = true; }
    public bool CheckChoice3B(string node, int lineIndex) { return choice3B; }

    public void SetTrueChoice4A(string node, int lineIndex) { choice4A = true; }
    public bool CheckChoice4A(string node, int lineIndex) { return choice4A; }
    public void SetTrueChoice4B(string node, int lineIndex) { choice4B = true; }
    public bool CheckChoice4B(string node, int lineIndex) { return choice4B; }

    public void ResetGlobalVariable()
    {
        choice1A = choice1B = choice2A = choice2B = false;
    }

}
