using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(RhythmLevel))]
public class RhythmLevelEditor : Editor {
    private ReorderableList _beats;

    private void OnEnable()
    {
        _beats = new ReorderableList(serializedObject, serializedObject.FindProperty("Beats"), true, true, true, true);

        _beats.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = _beats.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            int columnWidth = 80;
            int beatWidth = 200;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, columnWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Column"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + columnWidth, rect.y, beatWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Beat"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + columnWidth + beatWidth, rect.y, rect.width - columnWidth - beatWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Lyrics"), GUIContent.none);
        };

        _beats.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Beats");
        };
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
		_beats.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
    }
}