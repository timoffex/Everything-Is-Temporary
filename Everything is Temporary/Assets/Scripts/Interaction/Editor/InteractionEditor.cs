using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interaction))]
[CanEditMultipleObjects]
public class InteractionEditor : Editor
{
	SerializedObject conditionSO;
	
	public Condition condition; // TODO: Make it a list of conditions later.
	
	private void OnEnable()
	{
		interaction = (Interaction)target;
		
		conditionSO = new SerializedObject(interaction.condition);
		
		conditionProperties = new List<SerializedProperty>();
		
		/* Expose all properties for editing. */
		SerializedProperty conditionSP = conditionSO.GetIterator();
		conditionSP.NextVisible(true); // "You need to call Next (true) on the first element to get to the first element." --Unity3D
		SerializedProperty conditionEndSP = conditionSP.GetEndProperty();
		while (conditionSP != null) {
			string name = conditionSP.name;
			string type = conditionSP.type;
			
			// Skip default properties.
			if (name == "Base" || name == "m_Script") { goto Skip; }
			
			conditionProperties.Add(conditionSP);
			
			// Move to the next property if there's any.
			Skip:
				if (SerializedProperty.EqualContents(conditionSP, conditionEndSP)) { break; }
				conditionSP.NextVisible(true); // Next() need to be called very carefully.
		}
	}
	
	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		
		serializedObject.Update();
		
		for (int i = 0; i < conditionProperties.Count; i++)
		{
			EditorGUILayout.PropertyField(conditionProperties[i]);
		}
		
		serializedObject.ApplyModifiedProperties();
	}
	
	private Interaction interaction;
	
	private List<SerializedProperty> conditionProperties;
}

public class CustomProperty {
	
	
	
}
