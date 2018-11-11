using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WithinTrigger", menuName = "ScriptableObjects/Conditions/WithinTrigger", order = 1)]
public class WithinTrigger : Condition
{
	public GameObject triggerObject;
	public int lol;
	public string idk;
	
	public override bool isMet()
	{
		return false;
	}
}
