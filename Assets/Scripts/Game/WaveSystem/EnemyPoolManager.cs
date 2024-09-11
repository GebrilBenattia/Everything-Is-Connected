using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyPoolManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static EnemyPoolManager m_Instance;
    public static EnemyPoolManager instance
    {
        get { return m_Instance; }
    }

    // ######################################### VARIABLES ########################################

    // Pool Settings
    [Header("Pool Settings")]
    [SerializeField] private Transform m_PoolParentList;

    // Private Variables
    private List<EnemyBase> m_ActivePool = new List<EnemyBase>();
    private List<EnemyBase> m_InactivePool = new List<EnemyBase>();

    // ###################################### GETTER / SETTER #####################################

    public int activePoolSize
    {
        get { return m_ActivePool.Count; }
    }

    // ######################################### FUNCTIONS ########################################

    void Awake()
    {
        m_Instance = this;
    }

    private EnemyBase InstantiateActiveEnemy(GameObject _Prefab, Vector3 _Pos, Quaternion _Rotation)
    {
        // Instantiate an enemy
        GameObject instance = Instantiate(_Prefab, _Pos, _Rotation, m_PoolParentList);
        EnemyBase enemy = instance.GetComponent<EnemyBase>();
        enemy.Init();

        // Add enemy to Active Pool
        m_ActivePool.Add(enemy);

        return enemy;
    }

    public EnemyBase SpawnEnemy(GameObject _Prefab, Vector3 _Pos, Quaternion _Rotation)
    {
        // Return an unused newsObject in pool
        if (m_InactivePool.Count > 0)
        {
            // Extract enemyBase from inactive pool and add to active pool
            EnemyBase enemy = m_InactivePool[0];
            m_InactivePool.RemoveAt(0);
            m_ActivePool.Add(enemy);

            // Init newsObject
            enemy.gameObject.SetActive(true);
            enemy.transform.position = _Pos;
            enemy.transform.rotation = _Rotation;
            return enemy;
        }

        // Instantiate and return newsObject
        EnemyBase enemyInst = InstantiateActiveEnemy(_Prefab, _Pos, _Rotation);
        return enemyInst;
    }

    public void DespawnEnemy(EnemyBase _Enemy)
    {
        _Enemy.gameObject.SetActive(false);
        m_ActivePool.Remove(_Enemy);
        m_InactivePool.Add(_Enemy);
    }

    public void DespawnAllEnemies()
    {
        for (int i = 0; i < m_ActivePool.Count; ++i)
            DespawnEnemy(m_ActivePool[i]);
    }
}
