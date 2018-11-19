using UnityEngine;
using System.Collections;

public class TriggerAcquireItem : Trigger
{
    private void Awake()
    {
        // When an item is added, run InvokeTrigger()
        GameManager.Singleton.inventory.OnItemAdd += (item) =>
        {
            if (item.GetName() == m_requiredItemName)
                InvokeTrigger();
        };
    }

    [SerializeField]
    private string m_requiredItemName;
}
