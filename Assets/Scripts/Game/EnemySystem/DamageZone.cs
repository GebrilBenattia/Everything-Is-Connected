using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Zone Damage Settings
    [SerializeField] private int m_MaxDamageCycleCount;
    [SerializeField] private float m_DamageInterval;
    [SerializeField] private float m_DamageAmount;
    [SerializeField] private float m_ZoneRadius;
    [SerializeField] private LayerMask m_LayerMask;

    // Private Variables
    private float m_DamageIntervalCooldown;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_DamageIntervalCooldown = m_DamageInterval;
    }

    private void DealZoneDamage()
    {
        // Create overlap sphere and get all hit colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_ZoneRadius, m_LayerMask);

        // Loop over each hit colliders
        for (int i = 0; i < hitColliders.Length; i++) {

            // If is enemy: take damage
            if (hitColliders[i].gameObject.TryGetComponent(out EnemyBase enemy)) {
                enemy.TakeDamage(m_DamageAmount);
            }
        }

        --m_MaxDamageCycleCount;
    }

    private void Update()
    {
        if (m_MaxDamageCycleCount == 0) Destroy(gameObject);

        // Update damage interval cooldown
        if (m_DamageIntervalCooldown > 0) m_DamageIntervalCooldown -= Time.deltaTime;
        // Deal damage
        else {
            m_DamageIntervalCooldown += m_DamageInterval;
            DealZoneDamage();
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, m_ZoneRadius);
    }

#endif
}
