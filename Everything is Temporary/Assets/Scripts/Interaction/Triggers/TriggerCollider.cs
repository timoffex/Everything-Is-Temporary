using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TriggerCollider : Trigger
{

    private void Awake()
    {
        if (!GetComponent<Collider2D>().isTrigger)
            Debug.LogWarningFormat("Collider on {0} is not a trigger, " +
                                   "but a TriggerCollider script is attached.",
                                   gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(m_tag))
            return;

        InvokeTrigger();
    }

    [SerializeField]
    [Tooltip("This trigger will only fire when an object of this tag enters it.")]
    private string m_tag = "Player";
}
