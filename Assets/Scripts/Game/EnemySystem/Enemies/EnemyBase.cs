using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Enemy Settings
    [Header("Enemy Settings")]
    [SerializeField] protected int m_TokenCost;
    [SerializeField] protected float m_Life;
    [SerializeField] protected float m_Speed;
    [SerializeField] protected float m_Damage;

    // ######################################### FUNCTIONS ########################################

    protected abstract void EventOnWebCollision();
    protected abstract void EventOnDeath();

    protected void Death()
    {
        EventOnDeath();
        gameObject.SetActive(false);
    }

    protected void TakeDamage(int _DmageCount)
    {
        m_Life -= _DmageCount;
        if (m_Life <= 0 ) Death();
    }

    private void OnTriggerEnter(Collider _Other)
    {
        if (_Other.tag == "Web") EventOnWebCollision();
    }
    
    private void UpdateMovements()
    {
        transform.position += transform.forward * m_Speed * Time.deltaTime;
    }

    private void Update()
    {
        UpdateMovements();
    }
}
