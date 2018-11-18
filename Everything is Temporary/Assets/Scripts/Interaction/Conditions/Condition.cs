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
	
	protected virtual void Awake()
	{
		isMet = false;
		
		gameManager = GameManager.Singleton;
		m_conditionManager = GetComponent<ConditionManager>();
		
		m_conditionManager.conditions.Add(this);
	}
	
	protected bool isMet;
	
	protected GameManager gameManager;
	protected ConditionManager m_conditionManager;
	
}
