using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionHaveItem : Condition
{
    public string itemName;

    public override bool IsMet()
    {
        if (m_inventory.FindItemByName(itemName) != null)
            return true;
        else
            return false;
    }

    protected override void Awake()
    {
        base.Awake();

        m_inventory = m_gameManager.inventory;
    }

    private Inventory m_inventory;

}
