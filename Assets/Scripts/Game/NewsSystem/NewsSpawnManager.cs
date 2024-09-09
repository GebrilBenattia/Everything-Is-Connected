using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsSpawnManager : MonoBehaviour
{
    // SINGLETON
    // ---------
    private static NewsSpawnManager m_Instance;
    public static NewsSpawnManager instance 
    { get { return m_Instance; } }

    // VARIABLES
    // ---------
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 m_SpawnAreaSize;
    [SerializeField] private int m_MaxNewsCount;
    [SerializeField] private float m_MinSpawnTime;
    [SerializeField] private float m_MaxSpawnTime;
    [Header("News Data Settings")]
    [SerializeField] private GameObject m_NewsObjectPrefab;
    [SerializeField] private NewsData[] m_NewsDataList;
    private float m_CurrentSpawnCooldown;
    private int m_CurrentNewsNbr;

    // FUNCTIONS
    // ---------
    private void Awake()
    {
        m_Instance = this;
    }

    private void SpawnNews()
    {
        float posX = Random.Range(-m_SpawnAreaSize.x / 2f, m_SpawnAreaSize.x / 2f) + transform.position.x;
        float posZ = Random.Range(-m_SpawnAreaSize.z / 2f, m_SpawnAreaSize.z / 2f) + transform.position.z;
        GameObject newsObjectInstance = Instantiate(m_NewsObjectPrefab, new Vector3(posX, 0, posZ), Quaternion.identity);

        int randNewsDataIndex = Random.Range(0, m_NewsDataList.Length);
        newsObjectInstance.GetComponent<NewsObject>().Init(m_NewsDataList[randNewsDataIndex]);

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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw debug spawn area
        Gizmos.color = new Color(0, 0.8f, 1f, 0.5f);
        Gizmos.DrawCube(transform.position, m_SpawnAreaSize);
    }
#endif
}
