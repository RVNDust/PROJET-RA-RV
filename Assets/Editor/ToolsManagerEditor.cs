using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Valve.VR;

[CustomEditor(typeof (ToolsManager))]
public class ToolsManagerEditor : Editor {

	private ToolsManager settings;
	private ReorderableList toolsList;
	private Color color_selected;
	private Color color_index;
	void OnEnable()
	{
		settings = (ToolsManager)target;

		color_selected = new Color(0.349f, 0.537f, 0.811f);
		color_index = new Color(0.850f, 0.462f, 0.270f);

		toolsList = new ReorderableList(serializedObject, serializedObject.FindProperty("ToolsList"), true, true, true, true);

		toolsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			var element = toolsList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, rect.width - 64, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("ToolPrefab"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x + rect.width - 64, rect.y, 64, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("ToolIcon"), GUIContent.none);
		};

		toolsList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
		{
			if(isActive && isFocused) {
				EditorGUI.DrawRect(rect, color_selected);
			} 
			else if(index == settings.GetToolId()) 
			{
				EditorGUI.DrawRect(rect, color_index);
			}
		};

		toolsList.drawHeaderCallback = (Rect rect) =>
		{
			EditorGUI.LabelField(rect, "Tools list");
		};
	}
	override public void OnInspectorGUI()
	{
		serializedObject.Update();
		toolsList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

}
