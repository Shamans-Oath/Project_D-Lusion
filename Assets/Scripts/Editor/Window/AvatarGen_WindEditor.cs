//#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AvatarGen_WindEditor : EditorWindow
{

    [MenuItem("Tools/Avatar")]
    static public void Init()
    {
        EditorWindow.GetWindow<AvatarGen_WindEditor>(true, "Avatar builder", true).Show(true);
    }


    GameObject source;

    void OnGUI()
    {
        source = (GameObject)EditorGUILayout.ObjectField(source, typeof(GameObject), true);

        if (source)
        {
            Avatar av = AvatarBuilder.BuildGenericAvatar(source, source.name);
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.CreateAsset(av, "Assets/new Avatar.asset");
            }

        }
    }

}

//#endif