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
		
		m_gameManager = GameManager.Singleton;
		m_conditionManager = GetComponent<ConditionManager>();
		
		m_conditionManager.conditions.Add(this);
	}
	
	protected bool isMet;
	
	protected GameManager m_gameManager;
	protected ConditionManager m_conditionManager;
	
}
