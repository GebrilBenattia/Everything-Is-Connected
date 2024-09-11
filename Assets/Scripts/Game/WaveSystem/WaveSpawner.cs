using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private static WaveSpawner m_Instance;
    public static WaveSpawner Instance
    {
        get { return m_Instance; }
    }

    public Vector2 minCoords;
    public Vector2 maxCoords;

    [SerializeField] private List<Vector2> m_Left;
    [SerializeField] private List<Vector2> m_Bottom;
    [SerializeField] private List<Vector2> m_Right;
    [SerializeField] private List<Vector2> m_Top;

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    private Tuple<int, Vector2> GetRandomPosition()
    {
        int side = UnityEngine.Random.Range(0, 4);

        switch (side)
        {
            case 0:
                return Tuple.Create(0, new Vector2(minCoords.x, UnityEngine.Random.Range(minCoords.y, maxCoords.y)));
            case 1:
                return Tuple.Create(1, new Vector2(UnityEngine.Random.Range(minCoords.x, maxCoords.x), minCoords.y));
            case 2:
                return Tuple.Create(2, new Vector2(maxCoords.x, UnityEngine.Random.Range(minCoords.y, maxCoords.y)));
            case 3:
                return Tuple.Create(3, new Vector2(UnityEngine.Random.Range(minCoords.x, maxCoords.x), maxCoords.y));
            default:
                return Tuple.Create(0, new Vector2());
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Tuple<int, Vector2> randomPos = GetRandomPosition();

        FillSide(randomPos.Item1, randomPos.Item2);

        return new Vector3(randomPos.Item2.x, 3f, randomPos.Item2.y);
    }

    private void FillSide(int _Side, Vector2 _Position)
    {
        GetSideArray(_Side).Add(_Position);
    }

    private List<Vector2> GetSideArray(int _Side)
    {
        switch (_Side)
        {
            case 0:
                return m_Left;
            case 1:
                return m_Bottom;
            case 2:
                return m_Right;
            case 3:
                return m_Top;
            default:
                return new List<Vector2>();
        }
    }

    public void SpawnEnemy()
    {
        EnemyPoolManager.Instance.SpawnEnemy(GetSpawnPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("SPAWN");
            SpawnEnemy();
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(minCoords.x, 1f, minCoords.y), 0.3f);
        Gizmos.DrawSphere(new Vector3(maxCoords.x, 1f, maxCoords.y), 0.3f);
    }
#endif
}
