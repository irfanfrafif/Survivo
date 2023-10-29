using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Day3Sequence : MonoBehaviour
{
    [SerializeField] private bool seedPod;
    [SerializeField] private bool serum;
    [SerializeField] private bool liquidizer;
    [SerializeField] private bool food;
    [SerializeField] private bool window;
    [SerializeField] private bool radio;
    [SerializeField] private bool teleport;

    [SerializeField] private bool sleepPod;
    bool sleepPodTrigger;

    [SerializeField] private ObjectDialogue AIA;
    [SerializeField] private ObjectDialogue AIB;

    public void SetTrueSeedPod(string node, int lineIndex) { seedPod = true; }
    public void SetTrueSerum(string node, int lineIndex) { serum = true; }
    public void SetTrueLiquidizer(string node, int lineIndex) { liquidizer = true; }
    public void SetTrueFood(string node, int lineIndex) { food = true; }
    public void SetTrueWindow(string node, int lineIndex) { window = true; }
    public void SetTrueRadio(string node, int lineIndex) { radio = true; }
    public void SetTrueTeleport(string node, int lineIndex) { teleport = true; }


    public bool CheckSeedPod(string node, int lineIndex) { return seedPod; }
    public bool CheckSerum(string node, int lineIndex) { return serum; }
    public bool CheckLiquidizer(string node, int lineIndex) { return liquidizer; }
    public bool CheckFood(string node, int lineIndex) { return food; }
    public bool CheckWindow(string node, int lineIndex) { return window; }
    public bool CheckRadio(string node, int lineIndex) { return radio; }
    public bool CheckTeleport(string node, int lineIndex) { return teleport; }
    public bool CheckSleepPod(string node, int lineIndex) { return sleepPod; }

    private void Start()
    {
        StartCoroutine(AIACoroutine());
    }

    private void Update()
    {
        if (seedPod && serum && liquidizer && food && window && radio && teleport && !GlobalVariableTest.Instance.IsInDialogue) { sleepPod = true; }

        if (sleepPod && !sleepPodTrigger)
        {
            sleepPodTrigger = true;
            StartCoroutine(AIBCoroutine());          
        }
    }

    public void GoToNextScene(string node, int lineIndex)
    {
        GlobalVariableTest.Instance.IsInDialogue = true;
        SceneManager.LoadScene(4);
    }

    IEnumerator AIACoroutine()
    {
        yield return new WaitForSeconds(1f);

        AIA.StartConversation();
    }

    IEnumerator AIBCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        AIB.StartConversation();
    }
}
