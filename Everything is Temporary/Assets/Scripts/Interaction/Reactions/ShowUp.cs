using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates an object on React().
/// </summary>
public class ShowUp : Reaction
{
    public override void React()
    {
        m_objectToActivate.SetActive(true);
    }

    [SerializeField]
    [Tooltip("This reaction will activate this object.")]
    private GameObject m_objectToActivate;
}
