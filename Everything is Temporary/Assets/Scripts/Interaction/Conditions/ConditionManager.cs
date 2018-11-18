using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour {
	
	public List<Condition> conditions;
	
	public void CheckAllConditions()
	{
		foreach(Condition condition in conditions)
		{
			if (!condition.IsMet()) {
				Debug.Log("Some conditions aren't met.");
				return;
			}
		}
		
		Debug.Log("All conditions are met!");
	}

}
