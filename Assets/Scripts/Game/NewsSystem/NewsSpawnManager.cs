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

    // Spawn Settings
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 m_SpawnAreaSize;
    [SerializeField] private int m_MaxNewsCount;
    [SerializeField] private float m_MinSpawnTime;
    [SerializeField] private float m_MaxSpawnTime;

    // News Data Settings
    [Header("News Data Settings")]
    [SerializeField] private GameObject m_NewsObjectPrefab;
    [SerializeField] private NewsData[] m_NewsDataList;

    // Private Variables
    private float m_CurrentSpawnCooldown;
    private int[] m_NewsObjectCount = new int[4];
    private int m_TotalNewsObjectCount = 0;

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
        float[] spawnRates = new float[4];

        // Calculate Spawn Rates
        for (int i = 0; i < spawnRates.Length; ++i) {
            spawnRates[i] = m_TotalNewsObjectCount == 0 ? 1f : 1f / (1 + m_NewsObjectCount[i]);
        }

        // Normalize Spawn Rates
        float totalSpawnRate = 0f;
        for (int i = 0; i < spawnRates.Length; ++i) totalSpawnRate += spawnRates[i];
        for (int i = 0; i < spawnRates.Length; ++i) spawnRates[i] /= totalSpawnRate;

        Debug.Log("RATES: (1): " + spawnRates[0].ToString() + "(2): " + spawnRates[1].ToString() + "(3): " + spawnRates[2].ToString() + "(4): " + spawnRates[3].ToString());

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
            if (randFloat <= cumulativeRate) return i + 1;
        }

        return 0;
    }

    private void SpawnNewsObject()
    {
        // Get zone Index
        int zoneIndex = ChooseZone(CalculateSpawnRates());

        Debug.Log("ZONE " + zoneIndex.ToString());

        Vector2 minSpawnPoint = Vector2.zero;
        Vector2 maxSpawnPoint = Vector2.zero;
        switch (zoneIndex) {

            case 1: 
                minSpawnPoint = new Vector2(-m_SpawnAreaSize.x / 2f, m_SpawnAreaSize.z / 2f);
                maxSpawnPoint = Vector2.zero;
                break;

            case 2:
                minSpawnPoint = new Vector2(m_SpawnAreaSize.x / 2f, m_SpawnAreaSize.z / 2f);
                maxSpawnPoint = Vector2.zero;
                break;

            case 3:
                minSpawnPoint = new Vector2(-m_SpawnAreaSize.x / 2f, -m_SpawnAreaSize.z / 2f);
                maxSpawnPoint = Vector2.zero;
                break;

            case 4:
                minSpawnPoint = new Vector2(m_SpawnAreaSize.x / 2f, -m_SpawnAreaSize.z / 2f);
                maxSpawnPoint = Vector2.zero;
                break;
        }

        float posX = Random.Range(minSpawnPoint.x, maxSpawnPoint.x) + transform.position.x;
        float posZ = Random.Range(minSpawnPoint.y, maxSpawnPoint.y) + transform.position.z;

        int randNewsDataIndex = Random.Range(0, m_NewsDataList.Length);
        NewsObjectPoolManager.Instance.SpawnNewsObject(m_NewsDataList[randNewsDataIndex], new Vector3(posX, 0, posZ));

        m_TotalNewsObjectCount++;
        m_NewsObjectCount[zoneIndex - 1]++;
    }

    private void Update()
    {
        if (m_TotalNewsObjectCount >= m_MaxNewsCount) return;

        if (m_CurrentSpawnCooldown > 0) m_CurrentSpawnCooldown -= Time.deltaTime;
        else {
            m_CurrentSpawnCooldown += Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
            SpawnNewsObject();
        }
    }

    // Editor Functions
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        // Draw debug spawn area
        Gizmos.color = new Color(0, 0.8f, 1f, 0.5f);
        Gizmos.DrawCube(transform.position, m_SpawnAreaSize);
    }

#endif
}
