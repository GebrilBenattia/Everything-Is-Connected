using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class EnemyBase : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Enemy Settings
    [Header("Enemy Settings")]
    [SerializeField] protected float _spawnWaitTime;
    [SerializeField] protected float _life;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _damage;

    // Object References
    [Header("Prevent Sprite Settings")]
    [SerializeField] private float m_PreventSpriteOffset;
    [SerializeField] private GameObject m_PreventSpritePrefab;

    // VFX Settings
    [Header("VFX Settings")]
    [SerializeField] protected GameObject _webTrappedEffect;
    [SerializeField] private GameObject m_DeathPrefab;

    // Protected Variables
    protected Vector3 _initialPos;
    protected bool _isDead = false;
    protected bool _isSpawned = false;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        _initialPos = transform.position;
    }

    public void Init()
    {
        _initialPos = transform.position;
        _isDead = false;
        Invoke(nameof(SpawnEnemy), _spawnWaitTime);
        EnemyPreventSprite enemyPreventSprite = Instantiate(m_PreventSpritePrefab, transform.position + transform.forward * m_PreventSpriteOffset + new Vector3(0, 0.01f, 0), transform.rotation).GetComponent<EnemyPreventSprite>();
        enemyPreventSprite.Init(_spawnWaitTime * 0.9f * 1f / _speed);
    }

    private void SpawnEnemy()
    {
        _isSpawned = true;
    }

    public void TakeDamage(float _DamageAmount)
    {
        if (_isDead || !_isSpawned) return;

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
        _isDead = true;
        Instantiate(m_DeathPrefab, transform.position, transform.rotation);
        EventOnDeath();
        EnemyPoolManager.instance.DespawnEnemy(this);
    }

    protected void DealPlayerDamage()
    {
        GameplayManager.Instance.life -= _damage;
        GameplayManager.Instance.life = Mathf.Clamp(GameplayManager.Instance.life, 0f, GameplayManager.Instance.maxLife);
        GameplayManager.Instance.PLayDamageSound();
        if (GameplayManager.Instance.life == 0) GameplayManager.Instance.GameOver();
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
        if (!_isDead && _isSpawned) UpdateMovements();
    }
}
