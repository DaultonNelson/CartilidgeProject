using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The script that manages all of the dialogue.
    /// </summary>
    public DialogueManager dialogueManager;
    /// <summary>
    /// The Dialogue Tree this trigger will read from.
    /// </summary>
    public DialogueTree tree;
    /// <summary>
    /// The speech bubble for this trigger.
    /// </summary>
    public GameObject speechBubble;
    /// <summary>
    /// Invokes when the player interacts with the Trigger
    /// </summary>
    public UnityEvent onTriggerInteracted;

    /// <summary>
    /// Return true if player is close to trigger, or false if not.
    /// </summary>
    private bool playerClose = false;
    #endregion

    private void Update()
    {
        if (dialogueManager.inConversation == false)
        {
            if (playerClose && Input.GetButtonDown("Jump"))
            {
                TriggerDialogue();
            } 
        }
    }

    /// <summary>
    /// Checks to see if the Speech Bubble should come back up after a conversation.
    /// </summary>
    public void CheckForSpeechBubbleReshow()
    {
        if (playerClose)
        {
            speechBubble.SetActive(true);
        }
    }

    /// <summary>
    /// Triggers the Dialogue conversation.
    /// </summary>
    public void TriggerDialogue()
    {
        onTriggerInteracted.Invoke();
        dialogueManager.StartDialogue(tree.GetPickedDialogue());
        speechBubble.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !dialogueManager.inConversation)
        {
            speechBubble.SetActive(true);
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speechBubble.SetActive(false);
            playerClose = false;
        }
    }
}