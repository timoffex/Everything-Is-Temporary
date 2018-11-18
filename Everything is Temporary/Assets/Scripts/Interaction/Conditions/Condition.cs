using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour {
	
	public bool isTriggeringCondition;
	
	// Always override.
	public virtual bool IsMet()
	{
		return isMet;
	}
	
	private void Awake()
	{
		isTriggeringCondition = false;
		isMet = false;
	}
	
	protected bool isMet;
	
}
