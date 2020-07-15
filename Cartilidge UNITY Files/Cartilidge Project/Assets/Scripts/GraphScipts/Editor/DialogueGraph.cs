using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

namespace DialogueEditorGraph {
    public class DialogueGraph : EditorWindow {
        #region Variables
        private DialogueGraphView _graphView;
        private string _fileName = "New Narrative";
        #endregion

        [MenuItem("Graph/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            var window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent(text: "Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(_graphView);
            //Blackboard section is basically a header label
            blackboard.Add(new BlackboardSection { title = "Exposed Properties" });
            blackboard.addItemRequested = blackboard1 =>
            {
                _graphView.AddPropertyToBlackBoard(new ExposedProperty());
            };
            blackboard.editTextRequested = (blackboard1, element, newValue) =>
            {
                var oldProperty = ((BlackboardField)element).text;
                if (_graphView.exposedProperties.Any(x => x.propertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error!", "This property name already exists, please choose another one!", "OK");
                    return;
                }

                var propertyIndex = _graphView.exposedProperties.FindIndex(x => x.propertyName == oldProperty);
                _graphView.exposedProperties[propertyIndex].propertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };

            blackboard.SetPosition(new Rect(10, 3, 200, 300));
            _graphView._blackboard = blackboard;
            _graphView.Add(blackboard);
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap { anchored = true };
            //This will give 10 px offset from left side
            var coords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
            _graphView.Add(miniMap);
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView(this)
            {
                name = "Dialogue Graph"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField(label: "File Name");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true))
            {
                text = "Save Data"
            });

            toolbar.Add(new Button(() => RequestDataOperation(false))
            {
                text = "Load Data"
            });

            //var nodeCreateButton = new Button(clickEvent: () =>
            //{
            //    _graphView.CreateNode("Dialogue Node");
            //});
            //nodeCreateButton.text = "Create Node";
            //toolbar.Add(nodeCreateButton);

            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
            {
                saveUtility.SaveGraph(_fileName);
            }
            else
            {
                saveUtility.LoadGraph(_fileName);
            }
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    } 
}