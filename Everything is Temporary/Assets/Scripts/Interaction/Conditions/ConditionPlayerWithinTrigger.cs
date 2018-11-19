using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This condition is true when the player is inside the trigger collider
/// attached to this object.
/// </summary>
public class ConditionPlayerWithinTrigger : Condition
{
    public override bool IsMet()
    {
        return m_isInsideTrigger;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            m_isInsideTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            m_isInsideTrigger = false;
        }
    }

    private bool m_isInsideTrigger;
}
