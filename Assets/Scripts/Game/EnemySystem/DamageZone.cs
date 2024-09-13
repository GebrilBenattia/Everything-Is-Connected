using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DamageZone : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Zone Damage Settings
    [Header("Zone Damage Settings")]
    [SerializeField] private float m_DespawnTime;
    [SerializeField] private float m_DamageAmount;
    [SerializeField] private float m_ZoneRadius;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private VisualEffect m_Explosion;
    // Private Variables
    private EnemyBase m_EnemySource;
    private float m_DamageIntervalCooldown;

    // ######################################### FUNCTIONS ########################################

    public void Init(EnemyBase _EnemySource)
    {
        m_EnemySource = _EnemySource;
        DealZoneDamage();
        Invoke(nameof(Despawn), m_DespawnTime);
    }

    private void Start()
    {
        Debug.Log("created");
        m_Explosion.Play();
    }

    private void DealZoneDamage()
    {
        // Create overlap sphere and get all hit colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_ZoneRadius, m_LayerMask);

        // Loop over each hit colliders
        for (int i = 0; i < hitColliders.Length; i++) {

            // If is enemy: take damage
            if (hitColliders[i].gameObject.TryGetComponent(out EnemyBase enemy)) {
                if (m_EnemySource != enemy) enemy.TakeDamage(m_DamageAmount);
            }
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, m_ZoneRadius);
    }

#endif
}
