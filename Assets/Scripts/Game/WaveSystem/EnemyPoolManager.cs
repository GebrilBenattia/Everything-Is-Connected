using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyPoolManager : MonoBehaviour
{
    private static EnemyPoolManager m_Instance;
    public static EnemyPoolManager Instance
    {
        get { return m_Instance; }
    }

    [SerializeField] private uint m_MaxEnemyCount;
    private List<GameObject> m_ActivePool = new List<GameObject>();

    public int activePoolSize
    {
        get { return m_ActivePool.Count; }
    }

    private List<GameObject> m_InactivePool = new List<GameObject>();

    public List<GameObject> activePool
    {
        get { return m_ActivePool; }
    }

    public List<GameObject> inactivePool
    {
        get { return m_InactivePool; }
    }

    public GameObject enemyPrefab;

    [SerializeField] private Transform m_PoolParentList;

    void Awake()
    {
        m_Instance = this;
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < m_MaxEnemyCount; i++)
        {
            InstantiateInactiveEnemy();
        }
    }

    private GameObject InstantiateActiveEnemy(Vector3 _Pos)
    {
        // Instantiate an active Enemy
        GameObject instance = Instantiate(enemyPrefab, _Pos, Quaternion.identity, m_PoolParentList);
        //GameObject enemy = instance.GetComponent<GameObject>();
        //Init Enemy

        // Add NewsObject to Active Pool
        m_ActivePool.Add(instance);

        return instance;
    }

    private GameObject InstantiateInactiveEnemy()
    {
        // Instantiate an inactive Enemy
        GameObject instance = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, m_PoolParentList);
        //GameObject enemy = instance.GetComponent<GameObject>();
        instance.SetActive(false);

        // Add Enemy to Inactive Pool
        m_InactivePool.Add(instance);

        return instance;
    }

    public GameObject SpawnEnemy(Vector3 _Pos)
    {
        // Return an unused newsObject in pool
        if (m_InactivePool.Count > 0)
        {
            // Extract newsObject from inactive pool and add to active pool
            GameObject enemy = m_InactivePool[0];
            m_InactivePool.RemoveAt(0);
            m_ActivePool.Add(enemy);

            // Init newsObject
            enemy.gameObject.SetActive(true);
            enemy.transform.position = _Pos;
            return enemy;
        }

        // Instantiate and return newsObject
        GameObject newActiveEnemy = InstantiateActiveEnemy(_Pos);
        return newActiveEnemy;
    }

    public void DespawnEnemy(GameObject _Enemy)
    {
        _Enemy.SetActive(false);
        m_ActivePool.Remove(_Enemy);
        m_InactivePool.Add(_Enemy);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
