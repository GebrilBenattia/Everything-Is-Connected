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

    protected override void EventOnWebCollision(WebSegment _WebSegment)
    {
        if (_WebSegment.damage > m_MaxDmgImmunityAmount) TakeDamage(_WebSegment.damage);
    }

    protected override void EventOnDeath()
    {
    }
}
