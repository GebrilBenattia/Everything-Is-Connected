using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static WaveManager m_Instance;
    public static WaveManager instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

#if UNITY_EDITOR

    // Debug Settings
    [Header("Debug Settings")]
    [SerializeField] private bool m_ShowDebugLog;

#endif

    // Wave Settings
    [Header("Wave Settings")]
    [SerializeField] private float m_TimeBetweenWaves;
    [SerializeField] private int m_MaxTokenCount;
    [SerializeField] private int m_MaxActiveEnemies;
    [SerializeField] private int m_DifficultyIncrement;

    // Wave Spawn Settings
    [Header("Wave Spawn Settings")]
    [SerializeField] private float m_MinSpawnInterval;
    [SerializeField] private float m_MaxSpawnInterval;
    [SerializeField] private EnemyWaveData[] m_EnemyWaveDataList;

    // Private Variables
    private bool m_WaveIsInitialized = false;
    private int m_CurrentTokenCount;
    private float m_SpawnCooldown;
    private float m_WaveIntervalCooldown;
    private int m_CurrentWaveIndex;
    private int m_DifficultyIndex;

    // ###################################### GETTER / SETTER #####################################

    public int difficultyIndex
    {
        get { return difficultyIndex; }
    }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
        Init();
    }

    private void Init()
    {
        m_WaveIntervalCooldown = m_TimeBetweenWaves;
    }

#if UNITY_EDITOR

    private void LogManagerInfo()
    {
        if (m_ShowDebugLog) {
            Debug.Log("IsWaveActive : " + m_WaveIsInitialized);
            Debug.Log("Time before next wave : " + m_WaveIntervalCooldown);
            Debug.Log("Time before next enemy spawn : " + m_SpawnCooldown);

            Debug.Log("Remaining spawn token : " + m_CurrentTokenCount);
            Debug.Log("Remaining active enemy : " + EnemyPoolManager.instance.activePoolSize);
        }
    }

#endif

    private bool IsWaitingNextWave()
    {
        // Update Wave Interval Cooldown
        if (m_WaveIntervalCooldown > 0) {
            m_WaveIntervalCooldown -= Time.deltaTime;
            return true;
        }

        // Else Init Wave
        if (!m_WaveIsInitialized) InitWave();

        return false;
    }

    private void InitWave()
    {
        m_WaveIsInitialized = true;
        m_CurrentTokenCount = m_MaxTokenCount;
        m_SpawnCooldown = Random.Range(m_MinSpawnInterval, m_MaxSpawnInterval);
    }

    private void FinishWave()
    {
        m_WaveIsInitialized = false;
        m_WaveIntervalCooldown = m_TimeBetweenWaves;
        m_MaxTokenCount += m_DifficultyIncrement;
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
        return m_CurrentTokenCount <= 0 && EnemyPoolManager.instance.activePoolSize == 0;
    }

    private bool HasReachMaxActiveEnemies()
    {
        return EnemyPoolManager.instance.activePoolSize >= m_MaxActiveEnemies;
    }

    private void SpawnEnemy()
    {
        // Create temporary Enemy Wave Data list
        List<EnemyWaveData> allowedEnemiesWaveData = new List<EnemyWaveData>();

        // Loop on each Enemy Wave Data
        for (int i = 0; i < m_EnemyWaveDataList.Length; ++i) { 

            // Add to allowed list if tokenCost <= currentTokenCount
            if (m_EnemyWaveDataList[i].tokenCost <= m_CurrentTokenCount)
                allowedEnemiesWaveData.Add(m_EnemyWaveDataList[i]);
        }

        // Get Random index
        int enemyTypeIndex = Random.Range(0, allowedEnemiesWaveData.Count);

        // Spawn Enemy
        m_CurrentTokenCount -= allowedEnemiesWaveData[enemyTypeIndex].tokenCost;
        WaveSpawnerManager.instance.SpawnEnemy(allowedEnemiesWaveData[enemyTypeIndex].prefab);
    }

    private void UpdateWave()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ClearActiveEnemy();
        }

        // Called when wave is finished
        if (IsWaveFinished()) {
            FinishWave();
            return;
        }

        // Return if the max active enemies is reach
        if (HasReachMaxActiveEnemies()) return;

        // Return if token is null
        if (m_CurrentTokenCount == 0) return;

        // Update Spawn Cooldown
        if (m_SpawnCooldown > 0) m_SpawnCooldown -= Time.deltaTime;
        // Else Spawn Enemy
        else {
            m_SpawnCooldown += Random.Range(m_MinSpawnInterval, m_MaxSpawnInterval);
            SpawnEnemy();
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        LogManagerInfo();
#endif

        if (IsWaitingNextWave()) return;
        else UpdateWave();
    }
}
