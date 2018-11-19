using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The base class for picking up general items.
// If some special items need to be picked up differently, inherit from it!
public class ItemPickup : MonoBehaviour {
	
	// Make myself an item.
	private void Awake()
	{
		m_gameManager = GameManager.Singleton;
		m_inventory = m_gameManager.inventory;
	}
	
	// If the player hits me, get picked up.
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			m_inventory.AddItem(m_item);
			
			Destroy(gameObject);
		}
	}
	
	[SerializeField] private Item m_item;
	
	private GameManager m_gameManager;
	private Inventory m_inventory;
}
