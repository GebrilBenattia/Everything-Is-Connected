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
    [SerializeField] private SpawnData[] m_SpawnDataList;

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    public void SpawnEnemy(GameObject _Prefab)
    {
        // Init Variables
        int spawnDataIndex = Random.Range(0, m_SpawnDataList.Length);
        Vector3 position = m_SpawnDataList[spawnDataIndex].GetRandomPos();
        Quaternion rotation = m_SpawnDataList[spawnDataIndex].baseRotation;

        // Spawn enemy
        EnemyPoolManager.instance.SpawnEnemy(_Prefab, position, rotation);
    }


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {

        // Loop on each spawn data 
        if (m_ShowSpawnDataGizmos) {

            for (int i = 0; i < m_SpawnDataList.Length; ++i) {

                // Set Color
                Gizmos.color = m_SpawnDataList[i].gizmosColor;

                // Draw Spheres
                Gizmos.DrawSphere(m_SpawnDataList[i].minPos, 0.15f);
                Gizmos.DrawSphere(m_SpawnDataList[i].maxPos, 0.15f);

                // Set Color
                Gizmos.color = Color.yellow;

                // Draw rotation lines
                float length = 0.3f;
                Gizmos.DrawLine(m_SpawnDataList[i].minPos, m_SpawnDataList[i].minPos + m_SpawnDataList[i].baseRotation * Vector3.forward * length);
                Gizmos.DrawLine(m_SpawnDataList[i].maxPos, m_SpawnDataList[i].maxPos + m_SpawnDataList[i].baseRotation * Vector3.forward * length);
            }
        }
    }

#endif
}
