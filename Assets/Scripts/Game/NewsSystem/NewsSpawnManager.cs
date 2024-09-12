using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsSpawnManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static NewsSpawnManager m_Instance;
    public static NewsSpawnManager instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

#if UNITY_EDITOR

    // Debug Settings
    [Header("Debug Settings")]
    [SerializeField] private bool m_ShowDebugLog;

#endif

    // Spawn Settings
    [Header("Spawn Settings")]
    [SerializeField] private NewsSpawnZone[] m_SpawnZoneList;
    [SerializeField] private int m_MaxNewsCount;
    [SerializeField] private float m_MinSpawnTime;
    [SerializeField] private float m_MaxSpawnTime;
    [SerializeField] private float m_PosY;

    // News Data Settings
    [Header("News Data Settings")]
    [SerializeField] private GameObject m_NewsObjectPrefab;
    [SerializeField] private NewsData[] m_NewsDataList;

    // Private Variables
    private float m_CurrentSpawnCooldown;

    // ###################################### GETTER / SETTER #####################################

    public GameObject newsObjectPrefab
    {  get { return m_NewsObjectPrefab; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
        m_CurrentSpawnCooldown = Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
    }
    
    private float[] CalculateSpawnRates()
    {
        // Variables
        float[] spawnRates = new float[m_SpawnZoneList.Length];

        // Calculate Spawn Rates
        for (int i = 0; i < spawnRates.Length; ++i) {
            spawnRates[i] = NewsObjectPoolManager.instance.activePoolCount == 0 ? 1f : 1f / (1 + m_SpawnZoneList[i].newsCount);
        }

        // Normalize Spawn Rates
        float totalSpawnRate = 0f;
        for (int i = 0; i < spawnRates.Length; ++i) totalSpawnRate += spawnRates[i];
        for (int i = 0; i < spawnRates.Length; ++i) spawnRates[i] /= totalSpawnRate;

#if UNITY_EDITOR
        if (m_ShowDebugLog)
            Debug.Log("RATES: (1): " + spawnRates[0].ToString() + "(2): " + spawnRates[1].ToString() + "(3): " + spawnRates[2].ToString() + "(4): " + spawnRates[3].ToString());
#endif

        return spawnRates;
    }

    private int ChooseZone(float[] _SpawnRates)
    {
        // Variables
        float randFloat = Random.value;
        float cumulativeRate = 0f;

        // Calculate spawn Zone 
        for (int i = 0; i < _SpawnRates.Length; ++i) {

            cumulativeRate += _SpawnRates[i];
            if (randFloat <= cumulativeRate) return i;
        }

        return 0;
    }

    private void SpawnNewsObject()
    {
        // Get zone Index
        int zoneIndex = ChooseZone(CalculateSpawnRates());

#if UNITY_EDITOR
        if (m_ShowDebugLog) Debug.Log("ZONE " + (zoneIndex + 1).ToString());
#endif

        // Random position
        float posX = Random.Range(-m_SpawnZoneList[zoneIndex].areaSize.x / 2f, m_SpawnZoneList[zoneIndex].areaSize.x / 2f) + m_SpawnZoneList[zoneIndex].transform.position.x;
        float posZ = Random.Range(-m_SpawnZoneList[zoneIndex].areaSize.z / 2f, m_SpawnZoneList[zoneIndex].areaSize.z / 2f) + m_SpawnZoneList[zoneIndex].transform.position.z;

        // Instantiate random newsObject
        int randNewsDataIndex = Random.Range(0, m_NewsDataList.Length);
        NewsObjectPoolManager.instance.SpawnNewsObject(m_NewsDataList[randNewsDataIndex], new Vector3(posX, m_PosY, posZ));
    }

    private void Update()
    {
        if (NewsObjectPoolManager.instance.activePoolCount >= m_MaxNewsCount) return;

        if (m_CurrentSpawnCooldown > 0) m_CurrentSpawnCooldown -= Time.deltaTime;
        else {
            m_CurrentSpawnCooldown += Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
            SpawnNewsObject();
        }
    }
}
