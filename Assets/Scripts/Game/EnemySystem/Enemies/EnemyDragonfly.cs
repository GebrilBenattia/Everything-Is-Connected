using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDragonfly : EnemyBase
{
    // ######################################### VARIABLES ########################################

    // Dragonfly Settings
    [Header("Dragonfly Settings")]
    [SerializeField] private float m_MaxDmgImmunityAmount;

    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebLine _WebLine)
    {
        Instantiate(_webTrappedEffect, transform.position, Quaternion.identity);
        TakeDamage(_WebLine.damage);
    }

    protected override void EventOnDeath()
    {
    }
}
