using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialogue", menuName = "New Dialogue")]
public class Dialogue : ScriptableObject
{
    #region Variables
    /// <summary>
    /// The name of the thing we're talking to.
    /// </summary>
    public string interlocutorName;
    /// <summary>
    /// The Sentences we will load into our Queue.
    /// </summary>
    [TextArea(3, 10)]
    public string[] sentences;
    #endregion
}