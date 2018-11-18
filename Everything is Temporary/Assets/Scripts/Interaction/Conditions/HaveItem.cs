using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveItem : Condition {
	
	public string itemName;
	
	public override bool IsMet()
	{
		if (gameManager.inventory.FindItemByName(itemName) != null) {
			isMet = true;
		}
		else {
			isMet = false;
		}
		
		Debug.Log("HaveItem(" + itemName + ")is " + isMet);
		return isMet;
	}
	
}
