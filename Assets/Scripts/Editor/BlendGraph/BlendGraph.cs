using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BlendGraph : EditorWindow
{
    private BlendGraphView _graphView;

    [MenuItem("Window/Combo Graph")]
    public static void OpenBlendGraphWindow()
    {
        var window = GetWindow<BlendGraph>();
        window.titleContent = new GUIContent("Combo Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void ConstructGraphView()
    {
        _graphView = new BlendGraphView
        {
            name = "Combo Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(() => { _graphView.CreateNode("Combo Node"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }


    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
}
