using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interaction))]
[CanEditMultipleObjects]
public class InteractionEditor : Editor
{
	SerializedObject conditionSO;
	
	public Condition polyMorphicCondition; // TODO: Make it a list of conditions later.
	
	public GameObject GO;
	
	private void OnEnable()
	{

	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		serializedObject.Update();
		
		// Get the condition filled in by user.
		polyMorphicCondition = (Condition)(serializedObject.FindProperty("condition").objectReferenceValue);
		
		// Draw nothing extra if the condition isn't set.
		if (polyMorphicCondition == null) { return; }
		
		// Initialize.
		conditionSO = new SerializedObject(polyMorphicCondition);
		conditionProperties = new List<SerializedProperty>();
		
		ExposeConditionProperties();
		
		/* Add all editable properties to the editor. */
		foreach (SerializedProperty property in conditionProperties)
		{
			string name = property.name;
			string type = property.type;
			
			if (type == "PPtr<$GameObject>")
			{
				GO = (GameObject)EditorGUILayout.ObjectField(name, GO, typeof(GameObject), true);
			}
		}
		
		serializedObject.ApplyModifiedProperties();
	}
	
	private void ExposeConditionProperties()
	{
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
	
	private Interaction interaction;
	
	private List<SerializedProperty> conditionProperties;
}
