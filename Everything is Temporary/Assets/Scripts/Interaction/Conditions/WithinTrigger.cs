using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinTrigger : Condition {
	
	public override bool IsMet()
	{
		Debug.Log("WithinTrigger is " + isMet);
		return isMet;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			isMet = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			isMet = false;
		}
	}
	
}
