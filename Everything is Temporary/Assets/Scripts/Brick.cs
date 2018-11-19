using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {
	
	// Make myself an item.
	private void Awake()
	{
		m_item = new Item(GetComponent<SpriteRenderer>().sprite, "Brick", 1);
		
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
	
	private Item m_item;
	
	private GameManager m_gameManager;
	private Inventory m_inventory;
}
