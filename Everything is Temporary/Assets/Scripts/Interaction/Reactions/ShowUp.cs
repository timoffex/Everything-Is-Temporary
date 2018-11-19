using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This is a temporary script for playtest report!!!
public class ShowUp : Reaction {
	
	public override void React()
	{
		// Render self.
		GetComponent<SpriteRenderer>().enabled = true;
		
		// Enable all the child game objects.
		foreach (Transform childTransform in transform)
		{
			GameObject childGO = childTransform.gameObject;
			
			childGO.SetActive(true);
		}
	}
	
}
