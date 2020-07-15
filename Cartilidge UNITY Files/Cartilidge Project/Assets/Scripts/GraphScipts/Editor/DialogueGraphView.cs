using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor;

namespace DialogueEditorGraph {
    public class DialogueGraphView : GraphView {
        #region Variables
        /// <summary>
        /// Our exposed properties.
        /// </summary>
        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();
        /// <summary>
        /// The BlackBoard instance.
        /// </summary>
        public Blackboard _blackboard;

        /// <summary>
        /// The default scale of created nodes.
        /// </summary>
        public readonly Vector2 defaultNodeSize = new Vector2(x: 150, y: 200);

        /// <summary>
        /// The search window instance.
        /// </summary>
        private NodeSearchWindow _searchWindow;
        #endregion

        public DialogueGraphView(EditorWindow editorWindow)
        {
            styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "DialogueGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //Stuff for mouse drag and drop features
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();

            AddElement(GenerateEntryPoint());
            AddSearchWindow(editorWindow);
        }

        public void ClearBlackboardAndExposedProperties()
        {
            exposedProperties.Clear();
            _blackboard.Clear();
        }

        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
        {
            var localPropertyName = exposedProperty.propertyName;
            var localPropertyValue = exposedProperty.propertyValue;
            while (exposedProperties.Any(x => x.propertyName == localPropertyName))
            {
                localPropertyName = $"{localPropertyName}(1)";
            }

            var property = new ExposedProperty();
            property.propertyName = localPropertyName;
            property.propertyValue = localPropertyValue;
            exposedProperties.Add(property);

            var container = new VisualElement();
            var blackBoardField = new BlackboardField
            {
                text = property.propertyName,
                typeText = "String property"
            };
            container.Add(blackBoardField);

            var propertyValueTextField = new TextField("Value:")
            {
                value = localPropertyValue
            };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var changingPropertyIndex = exposedProperties.FindIndex(x => x.propertyName == property.propertyName);
                exposedProperties[changingPropertyIndex].propertyValue = evt.newValue;
            });
            var blackBoardValueRow = new BlackboardRow(blackBoardField, propertyValueTextField);
            container.Add(blackBoardValueRow);

            _blackboard.Add(container);
        }

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Init(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(funcCall: (port) =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            //Type is a bit arbitrary in this case
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, type: typeof(float));
        }

        private DialogueNode GenerateEntryPoint()
        {
            var node = new DialogueNode
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                dialogueText = "ENTRY POINT",
                entryPoint = true
            };

            var generatedPort = GeneratePort(node, Direction.Output);
            generatedPort.portName = "Next";
            node.outputContainer.Add(generatedPort);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));

            return node;
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            AddElement(CreateDialogueNode(nodeName, position));
        }

        public DialogueNode CreateDialogueNode(string nodeName, Vector2 position)
        {
            var dialogueNode = new DialogueNode
            {
                title = nodeName,
                dialogueText = nodeName,
                GUID = Guid.NewGuid().ToString()

            };

            var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            dialogueNode.inputContainer.Add(inputPort);

            dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            var button = new Button(clickEvent: () =>
            {
                AddChoicePort(dialogueNode);
            });
            button.text = "New Choice";
            dialogueNode.titleContainer.Add(button);

            var textField = new TextField(string.Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                dialogueNode.dialogueText = evt.newValue;
                dialogueNode.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(dialogueNode.title);
            dialogueNode.mainContainer.Add(textField);

            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
            dialogueNode.SetPosition(new Rect(position, defaultNodeSize));

            return dialogueNode;
        }

        public void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
        {
            var generatedPort = GeneratePort(dialogueNode, Direction.Output);

            //Q<>() is a generic function in which we can specify type and name we are looking for in this UI element
            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            var outputPortCount = dialogueNode.outputContainer.Query(name: "connector").ToList().Count;

            var choicePortName = string.IsNullOrEmpty(overridenPortName) ? $"Choice {outputPortCount + 1}" : overridenPortName;

            var textField = new TextField
            {
                name = string.Empty,
                value = choicePortName
            };
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);

            var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);

            generatedPort.portName = choicePortName;
            dialogueNode.outputContainer.Add(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
        }

        private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
        {
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            dialogueNode.outputContainer.Remove(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
        }
    } 
}