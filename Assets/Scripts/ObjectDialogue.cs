using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueGraph.Runtime;
using TMPro;
using Potato;

public class ObjectDialogue : Interactable
{
    public enum Facing
    {
        negativeX,
        positiveY,
        positiveX,
        negativeY,
    }

    [Header("Object")]
    [SerializeField] Facing facing;
    [SerializeField] Vector2Int gridPos;
    public Vector2Int frontGrid;

    [Header("Dialogue")]
    public RuntimeDialogueGraph DialogueSystem;
    public LineController LineController;

    [Header("UI Reference")]
    public GameObject SecondaryScreen;
    public GameObject PlayerContainer;
    public GameObject NpcContainer;
    public TMP_Text PlayerText;
    public TMP_Text NpcText;
    public TMP_Text NpcName;

    private bool startConversation = false;
    private bool isInConversation = false;
    private bool showingSecondaryScreen;
    private bool showPlayer;
    private bool isPlayerChoosing;
    private bool shouldShowText;
    private bool showingText;
    private string textToShow;

    public Vector2Int GetFrontGrid()
    {
        return gridPos + frontGrid;
    }
    private void Start()
    {
        gridPos = ((Vector2Int)ServiceLocator.Instance.gridManager.movementGrid.WorldToCell(gameObject.transform.position));

        switch (facing)
        {
            case Facing.positiveX:
                frontGrid = new Vector2Int(1, 0);
                break;
            case Facing.positiveY:
                frontGrid = new Vector2Int(0, 1);
                break;
            case Facing.negativeX:
                frontGrid = new Vector2Int(-1, 0);
                break;
            case Facing.negativeY:
                frontGrid = new Vector2Int(0, -1);
                break;
        }
    }

    private void Update()
    {
        Conversation();
    }

    public void StartConversation()
    {
        startConversation = true;
    }

    private void Conversation()
    {
        if (startConversation && !isInConversation)
        {
            startConversation = false;
            DialogueSystem.ResetConversation();
            isInConversation = true;
            (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);

            GlobalVariableTest.Instance.activeDialogue = this;
            GlobalVariableTest.Instance.IsInDialogue = isInConversation;
        }

        if (showingSecondaryScreen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                showingSecondaryScreen = false;
                SecondaryScreen.SetActive(false);
            }
            return;
        }

        if (!isInConversation || isPlayerChoosing) return;
        if (shouldShowText)
        {
            (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);
            (showPlayer ? PlayerText : NpcText).gameObject.SetActive(true);
            (showPlayer ? PlayerText : NpcText).text = textToShow;
            showingText = true;
            shouldShowText = false;
        }

        if (showingText)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                showingText = false;
                (showPlayer ? PlayerContainer : NpcContainer).SetActive(false);
                (showPlayer ? PlayerText : NpcText).gameObject.SetActive(false);
            }
        }
        else
        {
            if (DialogueSystem.IsConversationDone())
            {
                // Reset state
                isInConversation = false;
                showingSecondaryScreen = false;
                showPlayer = false;
                isPlayerChoosing = false;
                shouldShowText = false;
                showingText = false;

                GlobalVariableTest.Instance.IsInDialogue = isInConversation;

                PlayerContainer.SetActive(false);
                NpcContainer.SetActive(false);
                return;
            }

            var isNpc = DialogueSystem.IsCurrentNpc();
            if (isNpc)
            {
                var currentActor = DialogueSystem.GetCurrentActor();
                showPlayer = false;
                shouldShowText = true;
                textToShow = DialogueSystem.ProgressNpc();
                NpcName.text = currentActor.Name;
            }
            else
            {
                var currentLines = DialogueSystem.GetCurrentLines();
                isPlayerChoosing = true;
                PlayerContainer.SetActive(true);
                LineController.gameObject.SetActive(true);
                LineController.Initialize(currentLines);
            }
        }
    }

    public void PlayerSelect(int index)
    {
        LineController.gameObject.SetActive(false);
        DialogueSystem.ProgressSelf(index);
        //textToShow = DialogueSystem.ProgressSelf(index);
        isPlayerChoosing = false;
        //shouldShowText = true;
        //showPlayer = true;
    }

    protected override void InteractLogic()
    {
        StartConversation();
    }
}
