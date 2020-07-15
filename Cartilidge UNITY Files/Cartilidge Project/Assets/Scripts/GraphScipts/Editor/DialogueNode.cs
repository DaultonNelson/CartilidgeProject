using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace DialogueEditorGraph {
    public class DialogueNode : Node {
        #region Variables
        /// <summary>
        /// A unique ID to distinguish each node between each other.
        /// </summary>
        public string GUID;
        /// <summary>
        /// The dialogue text being displayed.
        /// </summary>
        public string dialogueText;
        /// <summary>
        /// Return true if node is the start up point of the conversation, or false if not.
        /// </summary>
        public bool entryPoint = false;
        #endregion
    } 
}