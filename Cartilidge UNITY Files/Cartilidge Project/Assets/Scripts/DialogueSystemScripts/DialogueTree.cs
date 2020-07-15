using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTree : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The current layer of the tree we're on.
    /// </summary>
    public int treeLayer = 1;
    /// <summary>
    /// The dialogues that will populate this tree.
    /// </summary>
    public List<Dialogue> dialogues = new List<Dialogue>();
    /// <summary>
    /// The object that stops the player in their tracks.
    /// </summary>
    public GameObject colliderStopper;


    /// <summary>
    /// The current game instance's information.
    /// </summary>
    private GameInstanceInformation instanceInformation;
    #endregion

    public void SceneInitialization()
    {
        StartCoroutine(GetInstanceInformation());
    }

    IEnumerator GetInstanceInformation()
    {
        yield return null;
        
        instanceInformation = FindObjectOfType<GameInstanceInformation>();
        treeLayer = instanceInformation.talkerTreeLayer;

        if (treeLayer > 2)
        {
            transform.root.gameObject.SetActive(false);
            colliderStopper.SetActive(false);
        }

        //Debug.Log(treeLayer);
    }

    /// <summary>
    /// Gets the dialogue the tree picks based off it's logic.
    /// </summary>
    /// <returns>
    /// The picked dialogue
    /// </returns>
    public Dialogue GetPickedDialogue()
    {
        Dialogue output = null;

        switch(treeLayer)
        {
            case 1:
                output = dialogues[0];
                treeLayer++;
                break;
            case 2:
                if (!instanceInformation.hasEye)
                {
                    output = dialogues[1];
                }
                else
                {
                    output = dialogues[2];
                    treeLayer++;
                }
                break;
            case 3:
                output = dialogues[3];
                break;
            default:
                Debug.LogError("DialogueTree is not at a valid layer value.", gameObject);
                break;
        }

        instanceInformation.talkerTreeLayer = treeLayer;
        return output;
    }
}