using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveItem : Condition {
	
	public string itemName;
	
	public override bool IsMet()
	{
		isMet = (gameManager.inventory.FindItemByName(itemName) != null) ? true : false;
		
		Debug.Log("HaveItem(" + itemName + ")is " + isMet);
		return isMet;
	}
	
	private void Awake()
	{
		gameManager = GameManager.Singleton;
	}
	
	private GameManager gameManager;
	
}
