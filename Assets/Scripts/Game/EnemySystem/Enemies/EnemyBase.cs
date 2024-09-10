using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Enemy Settings
    [Header("Enemy Settings")]
    [SerializeField] protected int _tokenCost;
    [SerializeField] protected float _life;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _damage;

    // Protected Variables
    protected Vector3 _initialPos;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _initialPos = transform.position;
    }

    protected abstract void EventOnWebCollision(WebSegment _WebSegment);
    protected abstract void EventOnDeath();

    protected virtual void EventOnBorderReach()
    {
        DealPlayerDamage();
        gameObject.SetActive(false);
    }

    protected void Death()
    {
        EventOnDeath();
        gameObject.SetActive(false);
    }

    protected void TakeDamage(float _DamageAmount)
    {
        _life -= _DamageAmount;
        if (_life <= 0 ) Death();
    }

    protected void DealPlayerDamage()
    {
        GameplayManager.Instance.life -= _damage;
    }

    private void OnTriggerEnter(Collider _Other)
    {
        if (_Other.CompareTag("WebSegment") && _Other.TryGetComponent(out WebSegment webSegment))
            EventOnWebCollision(webSegment);
        else if (_Other.CompareTag("MapBorder") && Vector3.Distance(transform.position, _initialPos) >= 0.1f)
            EventOnBorderReach();
    }
    
    private void UpdateMovements()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void Update()
    {
        UpdateMovements();
    }
}
