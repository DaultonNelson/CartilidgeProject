using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditorGraph {
    [Serializable]
    public class DialogueContainer : ScriptableObject {
        #region Variables
        /// <summary>
        /// A List of Node Link Data this container will hold.
        /// </summary>
        public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
        /// <summary>
        /// A List of Dialogue Node Data this container will hold.
        /// </summary>
        public List<DialogueNodeData> dialogueNodeDatas = new List<DialogueNodeData>();
        /// <summary>
        /// A List of Exposed Property this container will hold.
        /// </summary>
        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();
        #endregion
    } 
}