using System;
using UnityEngine;

namespace DialogueEditorGraph {
    [Serializable]
    public class DialogueNodeData {
        #region Variables
        /// <summary>
        /// The saved GUID of the node.
        /// </summary>
        public string nodeGUID;
        /// <summary>
        /// The saved dialogue text of the node.
        /// </summary>
        public string dialogueText;
        /// <summary>
        /// The saved position of the node.
        /// </summary>
        public Vector2 nodePosition;
        #endregion
    } 
}