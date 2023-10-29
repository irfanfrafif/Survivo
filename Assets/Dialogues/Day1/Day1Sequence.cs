using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Day1Sequence : MonoBehaviour
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

    [Header("Intro General")]
    [SerializeField] private Image fade;
    [SerializeField] private TMP_Text introHeader;

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
        GlobalVariableTest.Instance.IsInDialogue = true;

        Sequence startSequence = DOTween.Sequence();
        Sequence startSequenceText = DOTween.Sequence();

        startSequence.SetUpdate(true).AppendInterval(3f).Append(fade.DOFade(0f, 3f));

        startSequenceText.Append(introHeader.DOFade(1f, 2f)).AppendInterval(2f).Append(introHeader.DOFade(0f, 1f))
            .OnComplete(OnComplete).OnComplete(() => StartCoroutine(AIACoroutine()));

        //StartCoroutine(AIACoroutine());
    }

    void OnComplete()
    {
        fade.gameObject.SetActive(false);
        introHeader.gameObject.SetActive(false);
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

        Sequence startSequence = DOTween.Sequence();

        startSequence.SetUpdate(true).Append(fade.DOFade(1f, 3f)).AppendInterval(1f).OnComplete(() => SceneManager.LoadScene(2));
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
