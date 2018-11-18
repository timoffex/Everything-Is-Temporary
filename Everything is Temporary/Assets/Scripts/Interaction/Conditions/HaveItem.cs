using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveItem : Condition {
	
	public string itemName;
	
	public override bool IsMet()
	{
		if (m_inventory.FindItemByName(itemName) != null) {
			isMet = true;
		}
		else {
			isMet = false;
		}
		
		Debug.Log("HaveItem(" + itemName + ")is " + isMet);
		return isMet;
	}
	
	protected override void Awake()
	{
		base.Awake();
		
		m_inventory = m_gameManager.inventory;
		
		// Subscribe to Inventory.OnItemAdd event if...
		if (isTriggeringCondition)
		{
			m_inventory.OnItemAdd += CheckAddedItem;
		}
	}
	
	private void CheckAddedItem(Item item)
	{
		// Call conditionManager to check all conditions if...
		if (item.GetName() == itemName)
		{
			m_conditionManager.CheckAllConditions();
		}
	}
	
	private Inventory m_inventory;
	
}
