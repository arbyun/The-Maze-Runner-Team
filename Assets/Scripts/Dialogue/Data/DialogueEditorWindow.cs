using Dialogue.Graph;
using Dialogue.Models;
using Dialogue.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Data
{
    public class DialogueEditorWindow : EditorWindow
    {
        private string _filename = "New Conversation";

        private TextField _filenameTextField;
        private DialogueGraphView _graphView;
        private ObjectField _loadFileField;

        private void OnEnable()
        {
            AddGraphView();
            AddMiniMap();
            AddStyles();
            AddToolbar();
            AddSecondaryToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void OnDestroy()
        {
            AssetDatabase.SaveAssets();
        }

        [MenuItem("TMZ/Dialogue/Graph View #p", false, 0)]
        public static void Open()
        {
            Graphfl.Stm();
            GetWindow<DialogueEditorWindow>("Dialog Graph");
        }

        private void SaveData()
        {
            if (string.IsNullOrEmpty(_filename))
            {
                EditorUtility.DisplayDialog("Invalid filename", "Please enter a valid filename", "Ok");
                return;
            }

            var saveUtility = GraphSaveUtilities.GetInstance(_graphView);
            var previousGraph = (DialogueContainer)_loadFileField.value;
            if (previousGraph != null)
                saveUtility.OverwriteGraph((DialogueContainer)_loadFileField.value);
            else
                saveUtility.SaveGraph(_filename);
        }

        private void LoadData(DialogueContainer dialogContainer)
        {
            if (string.IsNullOrEmpty(_filename))
            {
                EditorUtility.DisplayDialog("Invalid filename", "Please enter a valid filename", "Ok");
                return;
            }

            var loadUtility = GraphSaveUtilities.GetInstance(_graphView);
            loadUtility.LoadGraph(dialogContainer);
        }

        private void ClearData()
        {
            var choice = EditorUtility.DisplayDialogComplex(
                "Are you sure?",
                "This will clear everything. There's no turning back",
                "Yes",
                "Cancel",
                "");

            if (choice == 0)
            {
                var clearUtility = GraphSaveUtilities.GetInstance(_graphView);
                clearUtility.ClearAll();
                ResetTextfields();
            }
        }

        private void ResetTextfields()
        {
            _loadFileField.value = null;
            _filename = "New Conversation";
            _filenameTextField.SetValueWithoutNotify(_filename);
            _filenameTextField.MarkDirtyRepaint();
        }

        #region Elements Addition

        private void AddToolbar()
        {
            var toolbar = new Toolbar();

            _filenameTextField = NodeElementsUtilities.CreateTextField(
                "Filename:",
                "Filename:",
                evt => { _filename = evt.newValue; });
            _filenameTextField.AddClasses("prata-node_textfield",
                "prata-node_quote-textfield"
            );
            _filenameTextField.SetValueWithoutNotify(_filename);
            _filenameTextField.MarkDirtyRepaint();
            toolbar.Add(_filenameTextField);

            var saveButton = NodeElementsUtilities.CreateButton("Save Data", SaveData);
            var clearButton = NodeElementsUtilities.CreateButton("Clear All", ClearData);

            toolbar.Add(saveButton);
            toolbar.Add(clearButton);

            rootVisualElement.Insert(1, toolbar);
        }

        private void AddSecondaryToolbar()
        {
            var toolbar = new Toolbar();

            _loadFileField = NodeElementsUtilities.CreateObjectField<DialogueContainer>("Load Graph");
            _loadFileField.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                if (evt.newValue == null)
                {
                    ResetTextfields();
                    return;
                }

                _filename = evt.newValue.name;
                _filenameTextField.SetValueWithoutNotify(_filename);
                _filenameTextField.MarkDirtyRepaint();
                LoadData((DialogueContainer)_loadFileField.value);
            });

            toolbar.Add(_loadFileField);

            rootVisualElement.Insert(2, toolbar);
        }

        private void AddGraphView()
        {
            _graphView = new DialogueGraphView(this);
            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("PrataVariables");
        }

        private void AddMiniMap()
        {
            var miniMap = new MiniMap { anchored = true };
            miniMap.SetPosition(new Rect(10, 55, 200, 140));
            _graphView.Add(miniMap);
        }

        #endregion
    }
}