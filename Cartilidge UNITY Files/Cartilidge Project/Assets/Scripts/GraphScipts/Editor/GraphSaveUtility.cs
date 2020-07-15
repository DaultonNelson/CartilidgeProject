using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueEditorGraph {
    public class GraphSaveUtility {
        #region Variables
        /// <summary>
        /// The target Dialogue Graph View class.
        /// </summary>
        private DialogueGraphView _targetGraphView;
        /// <summary>
        /// The target Dialogue Container.
        /// </summary>
        private DialogueContainer _containerCache;
        /// <summary>
        /// The graph view's connections, which we can access quickly.
        /// </summary>
        private List<Edge> edges => _targetGraphView.edges.ToList();
        /// <summary>
        /// The graph view's nodes.
        /// </summary>
        private List<DialogueNode> nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
        #endregion

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            if (!SaveNodes(dialogueContainer))
            {
                return;
            }
            SaveExposedProperties(dialogueContainer);

            //Auto creates resources folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer)
        {
            dialogueContainer.exposedProperties.AddRange(_targetGraphView.exposedProperties);
        }

        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<DialogueContainer>(fileName);
            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
            CreateExposedProperties();
        }

        private void CreateExposedProperties()
        {
            //Clear existing properties on hot-reload
            _targetGraphView.ClearBlackboardAndExposedProperties();
            //Add properties from data.
            foreach (var exposedProperty in _containerCache.exposedProperties)
            {
                _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
            }
        }

        private void ClearGraph()
        {
            //Set entry points guid back from the save.  Discard existing guid.
            nodes.Find(x => x.entryPoint).GUID = _containerCache.nodeLinks[0].baseNodeGuid;

            foreach (var node in nodes)
            {
                if (node.entryPoint)
                {
                    continue;
                }

                edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                _targetGraphView.RemoveElement(node);
            }
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _containerCache.dialogueNodeDatas)
            {
                //We'll pass the position later on, so we can just use v2.zero for now
                var tempNode = _targetGraphView.CreateDialogueNode(nodeData.dialogueText, Vector2.zero);
                tempNode.GUID = nodeData.nodeGUID;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodeData.nodeGUID).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.portName));
            }
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var connections = _containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodes[i].GUID).ToList();
                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].targetNodeGuid;
                    var targetNode = nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(_containerCache.dialogueNodeDatas.First(x => x.nodeGUID == targetNodeGuid).nodePosition,
                        _targetGraphView.defaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);

            _targetGraphView.Add(tempEdge);
        }

        private bool SaveNodes(DialogueContainer dialogueContainer)
        {
            //If no connections were made
            if (!edges.Any())
            {
                return false;
            }

            var connectedPorts = edges.Where(x => x.input.node != null).ToArray();
            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as DialogueNode;
                var inputNode = connectedPorts[i].input.node as DialogueNode;

                dialogueContainer.nodeLinks.Add(new NodeLinkData
                {
                    baseNodeGuid = outputNode.GUID,
                    portName = connectedPorts[i].output.portName,
                    targetNodeGuid = inputNode.GUID
                });
            }

            foreach (var dialogueNode in nodes.Where(node => !node.entryPoint))
            {
                dialogueContainer.dialogueNodeDatas.Add(new DialogueNodeData
                {
                    nodeGUID = dialogueNode.GUID,
                    dialogueText = dialogueNode.dialogueText,
                    nodePosition = dialogueNode.GetPosition().position
                });
            }
            return true;
        }
    } 
}