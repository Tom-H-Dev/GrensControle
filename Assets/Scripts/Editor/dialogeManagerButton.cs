using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueManager))]
public class dialogeManagerButton : Editor
{
        
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueManager loadExcel = (DialogueManager)target;

        GUILayout.Label(" Reload Item DataBase", EditorStyles.boldLabel);
        if (GUILayout.Button("reload Items")) 
        {
            loadExcel.loadItemData();
        }
    }
}

