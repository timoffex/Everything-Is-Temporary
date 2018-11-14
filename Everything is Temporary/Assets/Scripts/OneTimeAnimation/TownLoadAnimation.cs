using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownLoadAnimation : MonoBehaviour {
	
	// This should be called when the town loads.
	private void Start()
	{
		m_animator = GetComponent<Animator>();
		
		//m_animator.SetTrigger("");
		
		enabled = false;
	}
	
	private Animator m_animator;
	
}
