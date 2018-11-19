using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterColliderAnimation : MonoBehaviour {
	
	public string animationTriggerName;
	
	private void Start()
	{
		m_animator = GetComponent<Animator>();
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			//m_animator.SetTrigger("");
		}
		
		enabled = false;
	}
	
	private Animator m_animator;
	
}
