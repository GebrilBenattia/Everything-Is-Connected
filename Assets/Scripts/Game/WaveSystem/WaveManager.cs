using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private uint m_CurrentWaveIndex;
    [SerializeField] private uint m_MaxSpawnToken;
    [SerializeField] private uint m_DifficultyIncrement;
    [SerializeField] private uint m_TimeBetweenWaves;
    [SerializeField] private uint m_MaxActiveEnemies;

    private uint m_SpawnToken;

    [SerializeField] private float m_MinSpawnTime;
    [SerializeField] private float m_MaxSpawnTime;

    private float m_CurrentCooldown;

    private void Awake()
    {
        InitWave();
    }
    private void InitWave()
    {
        m_SpawnToken = m_MaxSpawnToken;
        m_CurrentCooldown = Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
    }

    private void ChangeWave()
    {
        m_CurrentWaveIndex++;
        m_MaxSpawnToken += m_DifficultyIncrement;
        m_MaxActiveEnemies += m_DifficultyIncrement;
    }

    private void Update()
    {
        Debug.Log(m_CurrentCooldown);
        Debug.Log(m_SpawnToken);

        Debug.Log(EnemyPoolManager.instance.activePoolSize);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            EnemyPoolManager.instance.DespawnAllEnemies();
            m_SpawnToken = 0;
        }

        if (m_SpawnToken == 0 && EnemyPoolManager.instance.activePoolSize == 0)
        {
            ChangeWave();
            InitWave();
        }

        if (m_SpawnToken <= 0 || EnemyPoolManager.instance.activePoolSize >= m_MaxActiveEnemies)
            return;

        if (m_CurrentCooldown > 0)
        {
            m_CurrentCooldown -= Time.deltaTime;
        }
        else
        {
            m_CurrentCooldown += Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
            WaveSpawnerManager.Instance.SpawnEnemy();
            m_SpawnToken--;
        }


    }
}
