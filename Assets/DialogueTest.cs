using DialogueGraph.Runtime;
using TMPro;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public RuntimeDialogueGraph DialogueSystem;
    public LineController LineController;

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
}