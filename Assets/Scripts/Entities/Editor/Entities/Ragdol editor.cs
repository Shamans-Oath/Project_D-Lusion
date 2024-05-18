using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RagdolSetter))]
public class Ragdoleditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RagdolSetter activator = (RagdolSetter)target;
        if (GUILayout.Button("Force Activate"))
        {
            activator.RagdollSetActive(true);
        }
        if (GUILayout.Button("Force Deactivate"))
        {
            activator.RagdollSetActive(false);
        }

    }
}
