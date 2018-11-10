using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	[Range(0f, 0.1f)] public float movementSpeed;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
	{
		
	}
	
	private void Update()
	{
		float xMovement = Input.GetAxis("Horizontal");

        m_animator.SetFloat("MovementSpeed", xMovement);

		transform.Translate(xMovement * movementSpeed, 0, 0);
	}

    private Animator m_animator;
}
