using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinTrigger : Condition {
	
	public WithinTrigger(GameObject trigger)
	{
		m_trigger = trigger;
	}
	
	public override bool IsMet()
	{
		return false;
	}
	
	private GameObject m_trigger;
	private GameObject player;
	
}
