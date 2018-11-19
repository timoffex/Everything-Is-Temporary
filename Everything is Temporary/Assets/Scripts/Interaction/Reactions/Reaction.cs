using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reaction : MonoBehaviour
{
    public abstract void React();

    protected virtual void Awake()
    {
        m_reactionManager = GetComponent<ReactionManager>();

        m_reactionManager.reactions.Add(this);
    }

    protected ReactionManager m_reactionManager;

}
