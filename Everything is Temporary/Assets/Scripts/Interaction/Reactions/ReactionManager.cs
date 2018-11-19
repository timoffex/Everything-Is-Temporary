using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public List<Reaction> reactions;

    public void PlayAllReactions()
    {
        foreach (Reaction reaction in reactions)
        {
            reaction.React();
        }
    }

    private void Awake()
    {
        m_conditionManager = GetComponent<ConditionManager>();

        foreach (var trigger in m_triggers)
            trigger.OnTrigger += CheckConditionsAndPlay;
    }

    private void CheckConditionsAndPlay()
    {
        if (m_conditionManager == null || m_conditionManager.CheckAllConditions())
            PlayAllReactions();
    }

    [SerializeField]
    [Tooltip("When any of these triggers activate, the list of conditions" +
             " will be checked. If the conditions are met, the reactions" +
             " will all be played.")]
    private List<Trigger> m_triggers;

    private ConditionManager m_conditionManager;
}
