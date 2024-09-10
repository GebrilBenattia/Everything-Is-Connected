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
    private int m_CurrentNewsNbr;

    // ###################################### GETTER / SETTER #####################################

    public GameObject newsObjectPrefab
    {  get { return m_NewsObjectPrefab; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
        m_CurrentSpawnCooldown = Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
    }

    private void SpawnNews()
    {
        float posX = Random.Range(-m_SpawnAreaSize.x / 2f, m_SpawnAreaSize.x / 2f) + transform.position.x;
        float posZ = Random.Range(-m_SpawnAreaSize.z / 2f, m_SpawnAreaSize.z / 2f) + transform.position.z;
        int randNewsDataIndex = Random.Range(0, m_NewsDataList.Length);

        NewsObjectPoolManager.Instance.SpawnNewsObject(m_NewsDataList[randNewsDataIndex], new Vector3(posX, 0, posZ));

        m_CurrentNewsNbr++;
    }

    private void Update()
    {
        if (m_CurrentNewsNbr >= m_MaxNewsCount) return;

        if (m_CurrentSpawnCooldown > 0) m_CurrentSpawnCooldown -= Time.deltaTime;
        else {
            m_CurrentSpawnCooldown += Random.Range(m_MinSpawnTime, m_MaxSpawnTime);
            SpawnNews();
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
