using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class EnemyBase : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Enemy Settings
    [Header("Enemy Settings")]
    [SerializeField] protected float _life;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _damage;

    // Protected Variables
    protected Vector3 _initialPos;

    // VFX variables
    [SerializeField] protected GameObject _webTrappedEffect; 

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        _initialPos = transform.position;
    }

    public void Init()
    {
        _initialPos = transform.position;
    }

    public void TakeDamage(float _DamageAmount)
    {
        _life -= _DamageAmount;
        if (_life <= 0) Death();
    }

    protected abstract void EventOnWebCollision(WebLine _WebLine);
    protected abstract void EventOnDeath();

    protected virtual void EventOnBorderReach()
    {
        DealPlayerDamage();
        EnemyPoolManager.instance.DespawnEnemy(this);
    }

    protected void Death()
    {
        EventOnDeath();
        EnemyPoolManager.instance.DespawnEnemy(this);
    }

    protected void DealPlayerDamage()
    {
        GameplayManager.Instance.life -= _damage;
        UIManager.instance.UpdateLife((int)GameplayManager.Instance.life);
    }

    private void OnTriggerEnter(Collider _Other)
    {
        if (_Other.CompareTag("WebLine") && _Other.TryGetComponent(out WebLine webLine))
            EventOnWebCollision(webLine);
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
