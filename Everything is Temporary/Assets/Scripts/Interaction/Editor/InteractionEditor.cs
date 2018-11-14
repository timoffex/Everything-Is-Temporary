/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interaction))]
//[CanEditMultipleObjects]
public class InteractionEditor : Editor
{
	private void OnEnable()
	{
		interaction = (Interaction)target;
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		// Draw the conditiion type menu, and update the value.
		interaction.conditionTypeIndex = EditorGUILayout.Popup("Condition Type", interaction.conditionTypeIndex, conditionTypes);
		interaction.conditionType = conditionTypes[interaction.conditionTypeIndex];
		
		switch (interaction.conditionType)
		{
			case "WithinTrigger":
				interaction.tempGameObjects[0] = (GameObject)EditorGUILayout.ObjectField("Trigger", interaction.tempGameObjects[0], typeof(GameObject), true);
				
				break;
				
			case "HaveItem":
				
				
				break;
		}
		
		serializedObject.ApplyModifiedProperties();
	}
	
	private Interaction interaction;
	
	private static string[] conditionTypes = {"WithinTrigger", "HaveItem"};
}*/
