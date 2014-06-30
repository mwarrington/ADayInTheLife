using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(AnimationManager))]

public class AnimationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		AnimationManager controller = (AnimationManager)target;
		
		EditorGUIUtility.LookLikeControls();
		
		EditorGUILayout.BeginHorizontal();
			controller.rowCount = EditorGUILayout.IntField("Number of Rows:", controller.rowCount);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			controller.colCount = EditorGUILayout.IntField("Number of Columns:", controller.colCount);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			controller.fps = EditorGUILayout.IntField("FPS:", controller.fps);
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Animation")) {
			controller.AddAnimation();
		}
		EditorGUILayout.EndHorizontal();
		
		if (controller.frameKeys != null)
		{
			for (int i = 0; i < controller.frameKeys.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical(new GUIStyle("box"));
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Animation "+(i+1)+":");
						controller.frameKeys[i] = EditorGUILayout.TextField(controller.frameKeys[i]);
					EditorGUILayout.EndHorizontal();
				
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Animation Type");
						if (controller.frameAnimTypes == null)
							controller.frameAnimTypes = new List<AnimationManager.AnimType>();
						while (controller.frameAnimTypes.Count <= i)
					controller.frameAnimTypes.Add(AnimationManager.AnimType.Loop);
				controller.frameAnimTypes[i] = (AnimationManager.AnimType)EditorGUILayout.EnumPopup(controller.frameAnimTypes[i]);
					EditorGUILayout.EndHorizontal();
					
					if (controller.frameSizeValues != null && i < controller.frameSizeValues.Count)
					{
						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Frame Width", GUILayout.MaxWidth(85));
							float x = EditorGUILayout.IntField((int)controller.frameSizeValues[i].x, GUILayout.MaxWidth(50));
							EditorGUILayout.LabelField("Frame Height", GUILayout.MaxWidth(85));
							float y = EditorGUILayout.IntField((int)controller.frameSizeValues[i].y, GUILayout.MaxWidth(50));
							controller.frameSizeValues[i] = new Vector2(x, y);
						EditorGUILayout.EndHorizontal();
					}
					
					if (controller.frameValues != null && i < controller.frameValues.values.Count)
					{
						for (int j = 0; j < controller.frameValues.values[i].values.Count; j++) {
							EditorGUILayout.BeginHorizontal();
								controller.frameValues.values[i].values[j] = EditorGUILayout.IntField("Frame Seq "+(j+1)+":", controller.frameValues.values[i].values[j]);
							EditorGUILayout.EndHorizontal();
						}
					}
					
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Add Key")) {
						controller.frameValues.values[i].values.Add(0);
					}
					if (GUILayout.Button("Remove Animation")) {
						controller.frameKeys.RemoveAt(i);
						controller.frameValues.values.RemoveAt(i);
					}
					EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}


