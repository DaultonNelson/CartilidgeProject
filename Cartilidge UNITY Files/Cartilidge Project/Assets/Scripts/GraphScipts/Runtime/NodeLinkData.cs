using System;


namespace DialogueEditorGraph {
    [Serializable]
    public class NodeLinkData {
        #region Variables
        /// <summary>
        /// The Guid of the base node.
        /// </summary>
        public string baseNodeGuid;
        /// <summary>
        /// The name of the port that links the two nodes together.
        /// </summary>
        public string portName;
        /// <summary>
        /// The Guid of the target node.
        /// </summary>
        public string targetNodeGuid;
        #endregion
    } 
}