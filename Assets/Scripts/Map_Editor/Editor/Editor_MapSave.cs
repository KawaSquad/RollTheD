using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapSave))]
public class Editor_MapSave: Editor
{
    public static MapSave scriptTarget = null;
    Vector2 scrollView;

    public int mLayer = 0;
    public override void OnInspectorGUI()
    {
        if (scriptTarget == null || scriptTarget != target)
            scriptTarget = (MapSave)target;

        if (scriptTarget != null && scriptTarget.PixelDatas != null)
            if (scriptTarget.PixelDatas.Length != (scriptTarget.sizeMap.x * scriptTarget.sizeMap.y))
                scriptTarget.ResetDatas();

        base.OnInspectorGUI();
        if (GUILayout.Button("Reset data array"))
        {
            if (scriptTarget != null)
                scriptTarget.ResetDatas();
        }
        if (GUILayout.Button("Generate Map"))
        {
            if (scriptTarget != null)
                scriptTarget.GenerateMap();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < 4; i++)//4 => Layer count
        {
            string textButton = "Layer_" + (i + 1);
            if (GUILayout.Button(textButton, (mLayer == i) ? EditorStyles.toolbarButton : EditorStyles.miniButton))
            {
                mLayer = i;
            }
        }
        EditorGUILayout.EndHorizontal();

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        EditorGUILayout.BeginVertical();

        //HEADER
        for (int y = scriptTarget.sizeMap.y - 1; y >= 0; y--) 
        {
            if (y == scriptTarget.sizeMap.y - 1)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < scriptTarget.sizeMap.x; x++)
                {
                    GUILayout.Label((x + 1).ToString());
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < scriptTarget.sizeMap.x; x++)
            {
                int index = y * scriptTarget.sizeMap.x + x;

                switch (mLayer)
                {
                    case 0:
                        scriptTarget.PixelDatas[index].indexL1 = EditorGUILayout.IntField(scriptTarget.PixelDatas[index].indexL1);
                        break;
                    case 1:
                        scriptTarget.PixelDatas[index].indexL2 = EditorGUILayout.IntField(scriptTarget.PixelDatas[index].indexL2);
                        break;
                    case 2:
                        scriptTarget.PixelDatas[index].indexL3 = EditorGUILayout.IntField(scriptTarget.PixelDatas[index].indexL3);
                        break;
                    case 3:
                        scriptTarget.PixelDatas[index].dataNav = EditorGUILayout.IntField(scriptTarget.PixelDatas[index].dataNav);
                        break;
                    default:
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        EditorUtility.SetDirty(scriptTarget);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }
}