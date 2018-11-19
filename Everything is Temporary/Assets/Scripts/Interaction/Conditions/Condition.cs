using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : MonoBehaviour
{
    public abstract bool IsMet();

    protected virtual void Awake()
    {
        m_gameManager = GameManager.Singleton;
        m_conditionManager = GetComponent<ConditionManager>();

        m_conditionManager.conditions.Add(this);
    }

    protected GameManager m_gameManager;
    protected ConditionManager m_conditionManager;

}
