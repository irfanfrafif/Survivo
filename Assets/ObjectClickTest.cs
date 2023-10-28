using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickTest : MonoBehaviour
{
    DialogueTest dialogue;

    private void OnMouseOver()
    {
        dialogue.StartConversation();
    }
}
