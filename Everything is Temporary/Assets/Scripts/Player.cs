using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	[Range(0f, 0.1f)] public float movementSpeed;

	private void Start()
	{
		
	}
	
	private void Update()
	{
		float xMovement = Input.GetAxis("Horizontal");
		
		transform.Translate(xMovement * movementSpeed, 0, 0);
	}

}
