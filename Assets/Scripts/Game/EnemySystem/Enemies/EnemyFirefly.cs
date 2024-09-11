using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFirfly : EnemyBase
{
    // ######################################### VARIABLES ########################################

    // Firefly Settings
    [Header("Firefly Settings")]
    [SerializeField] private GameObject m_DamageZonePrefab;

    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebSegment _WebSegment)
    {
        TakeDamage(_WebSegment.damage);
    }

    protected override void EventOnDeath()
    {
        Instantiate(m_DamageZonePrefab, transform.position, Quaternion.identity);
    }
}
