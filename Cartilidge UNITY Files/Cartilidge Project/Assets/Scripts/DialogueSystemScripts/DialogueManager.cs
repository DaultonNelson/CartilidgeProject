using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    #region Variables
    /// <summary>
    /// The player controller, Cartilidge.
    /// </summary>
    public CartilidgeController cartilidgeController;
    /// <summary>
    /// The GameObject that holds the Dialogue Box.
    /// </summary>
    public Animator dialogueBoxAnimator;
    /// <summary>
    /// The text field that will display the interlocutor name.
    /// </summary>
    public Text nameField;
    /// <summary>
    /// The text field that will display the said dialogue.
    /// </summary>
    public Text saidDialogueField;
    /// <summary>
    /// The image that represents the continue arrow.
    /// </summary>
    public Image continueArrow;
    /// <summary>
    /// Invokes functions once the conversation has ended.
    /// </summary>
    public UnityEvent onConversationEnded;

    /// <summary>
    /// Return true as to whether a conversation is taking place, or false if not.
    /// </summary>
    public bool inConversation { get; set; }

    //A Queue works in many ways like a List, but it is a bit more restricted.
    //A FIFO collection - First In, First Out
    /// <summary>
    /// The sentences that will display in our Dialogue box.
    /// </summary>
    private Queue<string> sentences;
    #endregion

    void Start()
    {
        sentences = new Queue<string>();
        nameField.text = string.Empty;
        saidDialogueField.text = string.Empty;
    }

    private void Update()
    {
        if (inConversation)
        {
            if (Input.GetButtonDown("Jump"))
            {
                DisplayNextSentence();
            }
        }
    }

    /// <summary>
    /// Starts the dialogue with the given trigger.
    /// </summary>
    /// <param name="trigger">
    /// The trigger we're talking to.
    /// </param>
    public void StartDialogue(Dialogue dialogue)
    {
        //Debug.Log($"Starting conversation with {dialogue.interlocutorName}");
        cartilidgeController.ToggleControllability(false);
        sentences.Clear();
        nameField.text = dialogue.interlocutorName;

        dialogueBoxAnimator.SetBool("IsOpen", true);

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Displays the next sentence in the dialogue box.
    /// </summary>
    public void DisplayNextSentence()
    {
        //Debug.Log("Display Next");

        continueArrow.enabled = false;

        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }

        string sentence = sentences.Dequeue();
        //saidDialogueField.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Types the sentence out letter by letter.
    /// </summary>
    /// <param name="sentence">
    /// The sentence this coroutine will be spelling out.
    /// </param>
    /// <returns>
    /// Nothing.
    /// </returns>
    IEnumerator TypeSentence(string sentence)
    {
        saidDialogueField.text = string.Empty;
        foreach (char character in sentence.ToCharArray())
        {
            saidDialogueField.text += character;
            yield return null;
        }
        inConversation = true;
        continueArrow.enabled = true;
    }

    IEnumerator EndDialogue()
    {
        nameField.text = string.Empty;
        saidDialogueField.text = string.Empty;
        dialogueBoxAnimator.SetBool("IsOpen", false);
        //Debug.Log("End of conversation");
        onConversationEnded.Invoke();
        
        yield return null;

        inConversation = false;
    }
}