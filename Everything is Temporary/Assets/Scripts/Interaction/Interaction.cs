using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
	public Condition condition;
	public int conditionTypeIndex;
	public string conditionType;
	public List<GameObject> tempGameObjects;
	
	private void Start()
	{
		switch (conditionType)
		{
			case "WithinTrigger":
				WithinTrigger withinTrigger = new WithinTrigger(tempGameObjects[0]);
				condition = withinTrigger;
			
				break;
				
			case "HaveItem":
			
				break;
				
			default:
			
				break;
		}
	}
	
	private void Update()
	{
		if (condition == null) { return; }
		
		if (condition.IsMet())
		{
			Debug.Log("Condition is met.");
		}
	}
}
