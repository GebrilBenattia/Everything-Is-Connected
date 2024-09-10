using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsObjectPoolManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static NewsObjectPoolManager m_Instance;
    public static NewsObjectPoolManager Instance
    {  get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

    // Pool Settings
    [Header("Pool Settings")]
    [SerializeField] private int m_InitSize;
    [SerializeField] private Transform m_PoolParentList;

    // Private Variables
    private List<NewsObject> m_ActivePool = new List<NewsObject>();
    private List<NewsObject> m_InactivePool = new List<NewsObject>();

    // ###################################### GETTER / SETTER #####################################

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // Create initial newsObject in Inactive Pool
        for (int i = 0; i < m_InitSize; i++) {
            InstantiateInactiveNewsObject();
        }
    }

    private NewsObject InstantiateActiveNewsObject(NewsData _NewsData, Vector3 _Pos)
    {
        // Instantiate a NewsObject
        GameObject instance = Instantiate(NewsSpawnManager.instance.newsObjectPrefab, _Pos, Quaternion.identity, m_PoolParentList);
        NewsObject newsObject = instance.GetComponent<NewsObject>();
        newsObject.Init(_NewsData);

        // Add NewsObject to Active Pool
        m_ActivePool.Add(newsObject);

        return newsObject;
    }

    private NewsObject InstantiateInactiveNewsObject()
    {
        // Instantiate a NewsObject
        GameObject instance = Instantiate(NewsSpawnManager.instance.newsObjectPrefab, Vector3.zero, Quaternion.identity, m_PoolParentList);
        NewsObject newsObject = instance.GetComponent<NewsObject>();
        instance.SetActive(false);

        // Add NewsObject to Active Pool
        m_InactivePool.Add(newsObject);

        return newsObject;
    }

    public NewsObject SpawnNewsObject(NewsData _NewsData, Vector3 _Pos)
    {
        // Return an unused newsObject in pool
        if (m_InactivePool.Count > 0) {

            // Extract newsObject from inactive pool and add to active pool
            NewsObject newsObject = m_InactivePool[0];
            m_InactivePool.RemoveAt(0);
            m_ActivePool.Add(newsObject);

            // Init newsObject
            newsObject.gameObject.SetActive(true);
            newsObject.transform.position = _Pos;
            newsObject.Init(_NewsData);
            return newsObject;
        }

        // Instantiate and return newsObject
        NewsObject newsObjectInst = InstantiateActiveNewsObject(_NewsData, _Pos);
        return newsObjectInst;
    }

    public void DespawnNewsObject(NewsObject _NewsObject)
    {
        // Release the newsObject and put it in the inactive pool
        _NewsObject.gameObject.SetActive(false);
        m_ActivePool.Remove(_NewsObject);
        m_InactivePool.Add(_NewsObject);
    }
}
