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

    protected override void EventOnWebCollision(WebLine _WebLine)
    {
        TakeDamage(_WebLine.damage);
    }

    protected override void EventOnDeath()
    {
        Instantiate(m_DamageZonePrefab, transform.position, Quaternion.identity);
    }
}
