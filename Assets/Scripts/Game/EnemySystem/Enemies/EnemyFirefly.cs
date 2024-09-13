using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyFirfly : EnemyBase
{
    // ######################################### VARIABLES ########################################

    // Firefly Settings
    [Header("Firefly Settings")]
    [SerializeField] private GameObject m_DamageZonePrefab;

    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebLine _WebLine)
    {
        Instantiate(_webTrappedEffect, transform.position, Quaternion.identity);
        TakeDamage(_WebLine.damage);
    }

    protected override void EventOnDeath()
    {
        Instantiate(m_DamageZonePrefab, transform.position, Quaternion.identity);
    }
}
