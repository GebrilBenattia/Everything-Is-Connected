using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager m_Instance;
    public static WaveManager instance
    {
        get { return m_Instance; }
    }

    [SerializeField] private uint m_CurrentWaveIndex;
    [SerializeField] private uint m_MaxSpawnToken;
    [SerializeField] private uint m_DifficultyIncrement;
    [SerializeField] private uint m_DifficultyIndex;

    public uint difficultyIndex
    {
        get { return difficultyIndex; }
    }

    [SerializeField] private uint m_TimeBetweenWaves;
    [SerializeField] private uint m_MaxActiveEnemies;

    private uint m_SpawnToken;

    [SerializeField] private Vector2 m_SpawnTimeInterval;

    private float m_CurrentSpawnCooldown;
    private float m_CurrentWaveInterval;

    private bool m_IsWaveActive;

    private void Awake()
    {
        m_Instance = this;
    }
    private void Start()
    {
        RetieveWaveInfo();
    }

    private void LogManagerInfo()
    {
        Debug.Log("IsWaveActive : " + m_IsWaveActive);
        Debug.Log("Time before next wave : " + m_CurrentWaveInterval);
        Debug.Log("Time before next enemy spawn : " + m_CurrentSpawnCooldown);

        Debug.Log("Remaining spawn token : " + m_SpawnToken);
        Debug.Log("Remaining active enemy : " + EnemyPoolManager.instance.activePoolSize);
    }

    private void RetieveWaveInfo()
    {
        m_IsWaveActive = false;
        m_CurrentWaveInterval = m_TimeBetweenWaves;

    }

    private bool WaitBetweenWaves()
    {
        if (m_CurrentWaveInterval > 0)
        {
            m_CurrentWaveInterval -= Time.deltaTime;
            return true;
        }

        if (!m_IsWaveActive)
        {
            BeginWave();
            m_IsWaveActive = true;
        }
        return false;
    }

    private void BeginWave()
    {
        m_SpawnToken = m_MaxSpawnToken;
        m_CurrentSpawnCooldown = Random.Range(m_SpawnTimeInterval.x, m_SpawnTimeInterval.y);
    }

    private void FinishWave()
    {
        m_CurrentWaveInterval = m_TimeBetweenWaves;
        m_MaxSpawnToken += m_DifficultyIncrement;
        m_MaxActiveEnemies += m_DifficultyIncrement;
        m_DifficultyIndex += m_DifficultyIncrement;
        m_CurrentWaveIndex++;
    }

    private void ClearActiveEnemy()
    {
        EnemyPoolManager.instance.DespawnAllEnemies();
    }

    private bool IsWaveFinished()
    {
        return m_SpawnToken == 0 && EnemyPoolManager.instance.activePoolSize == 0;
    }

    private void UpdateWave()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ClearActiveEnemy();
        }

        if (IsWaveFinished())
        {
            m_IsWaveActive = false;
            FinishWave();
        }

        if (m_SpawnToken <= 0 || EnemyPoolManager.instance.activePoolSize >= m_MaxActiveEnemies)
            return;

        if (m_CurrentSpawnCooldown > 0)
        {
            m_CurrentSpawnCooldown -= Time.deltaTime;
        }
        else
        {
            m_CurrentSpawnCooldown = Random.Range(m_SpawnTimeInterval.x, m_SpawnTimeInterval.y);
            WaveSpawnerManager.instance.SpawnEnemy();
            m_SpawnToken--;
        }
    }

    private void Update()
    {
        LogManagerInfo();

        if (!m_IsWaveActive && WaitBetweenWaves())
        {
            return;
        }
        else
        {
            UpdateWave();
        }
    }
}
