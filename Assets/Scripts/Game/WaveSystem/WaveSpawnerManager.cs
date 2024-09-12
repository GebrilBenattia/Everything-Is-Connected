using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerManager : MonoBehaviour
{
    // ########################################## STRUCTS #########################################

    [System.Serializable]
    public struct SpawnData
    {
#if UNITY_EDITOR

        [Header("Debug Settings")]
        public Color gizmosColor;

#endif

        // Variables
        [Header("Spawn Data Settings")]
        public Vector3 minPos;
        public Vector3 maxPos;
        public Quaternion baseRotation;

        // Functions
        public Vector3 GetRandomPos()
        {
            float randX = Random.Range(minPos.x, maxPos.x);
            float randY = Random.Range(minPos.y, maxPos.y);
            float randZ = Random.Range(minPos.z, maxPos.z);
            return new Vector3(randX, randY, randZ);
        }
    }

    // ######################################### SINGLETON ########################################

    private static WaveSpawnerManager m_Instance;
    public static WaveSpawnerManager instance
    {
        get { return m_Instance; }
    }

    // ######################################### VARIABLES ########################################

    // Debug Settings
    [Header("Debug Settings")]
    [SerializeField] private bool m_ShowSpawnDataGizmos;

    // Spawner Settings
    [Header("Spawner Settings")]
    [SerializeField] private float m_DefaultYPos;
    [SerializeField] private SpawnData m_LeftBorderSpawn = new SpawnData();
    [SerializeField] private SpawnData m_RightBorderSpawn = new SpawnData();
    [SerializeField] private SpawnData m_UpBorderSpawn = new SpawnData();
    [SerializeField] private SpawnData m_DownBorderSpawn = new SpawnData();

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        // Left Border
        m_LeftBorderSpawn.minPos = GameBorderManager.instance.worldLeftDown + Vector3.up * m_DefaultYPos;
        m_LeftBorderSpawn.maxPos = GameBorderManager.instance.worldLeftUp + Vector3.up * m_DefaultYPos;

        // Right Border
        m_RightBorderSpawn.minPos = GameBorderManager.instance.worldRightDown + Vector3.up * m_DefaultYPos;
        m_RightBorderSpawn.maxPos = GameBorderManager.instance.worldRightUp + Vector3.up * m_DefaultYPos;

        // Up Border
        m_UpBorderSpawn.minPos = GameBorderManager.instance.worldLeftUp + Vector3.up * m_DefaultYPos;
        m_UpBorderSpawn.maxPos = GameBorderManager.instance.worldRightUp + Vector3.up * m_DefaultYPos;

        // Down Border
        m_DownBorderSpawn.minPos = GameBorderManager.instance.worldLeftDown + Vector3.up * m_DefaultYPos;
        m_DownBorderSpawn.maxPos = GameBorderManager.instance.worldRightDown + Vector3.up * m_DefaultYPos;
    }

    private SpawnData GetSpawnDataFromIndex(int _Index)
    {
        switch(_Index) {
            case 0: return m_LeftBorderSpawn;
            case 1: return m_RightBorderSpawn;
            case 2: return m_UpBorderSpawn;
            case 3: return m_DownBorderSpawn;
            default: return m_LeftBorderSpawn;
        }
    }

    public void SpawnEnemy(GameObject _Prefab)
    {
        // Init Variables
        int spawnDataIndex = Random.Range(0, 4);
        SpawnData spawnData = GetSpawnDataFromIndex(spawnDataIndex);
        Vector3 position = spawnData.GetRandomPos();
        Quaternion rotation = spawnData.baseRotation;

        // Spawn enemy
        EnemyPoolManager.instance.SpawnEnemy(_Prefab, position, rotation);
    }


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {

        // Loop on each spawn data 
        if (m_ShowSpawnDataGizmos) {

            DrawSpawnDataGizmos(m_LeftBorderSpawn);
            DrawSpawnDataGizmos(m_RightBorderSpawn);
            DrawSpawnDataGizmos(m_UpBorderSpawn);
            DrawSpawnDataGizmos(m_DownBorderSpawn);
        }
    }

    private void DrawSpawnDataGizmos(SpawnData _SpawnData)
    {
        // Set Color
        Gizmos.color = _SpawnData.gizmosColor;

        // Draw Spheres
        Gizmos.DrawSphere(_SpawnData.minPos, 0.15f);
        Gizmos.DrawSphere(_SpawnData.maxPos, 0.15f);

        // Set Color
        Gizmos.color = Color.yellow;

        // Draw rotation lines
        float length = 0.3f;
        Gizmos.DrawLine(_SpawnData.minPos, _SpawnData.minPos + _SpawnData.baseRotation * Vector3.forward * length);
        Gizmos.DrawLine(_SpawnData.maxPos, _SpawnData.maxPos + _SpawnData.baseRotation * Vector3.forward * length);
    }

#endif
}
