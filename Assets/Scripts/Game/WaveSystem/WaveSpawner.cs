using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Vector2 minCoords;
    public Vector2 maxCoords;

    // Start is called before the first frame update
    void Start()
    {

    }

    private Vector2 GetRandomPosition()
    {
        int side = Random.Range(0, 4);

        Debug.Log(side);

        switch (side)
        {
            case 0:
                return new Vector2(minCoords.x, Random.Range(minCoords.y, maxCoords.y));
            case 1:
                return new Vector2(Random.Range(minCoords.x, maxCoords.x), minCoords.y);
            case 2:
                return new Vector2(maxCoords.x, Random.Range(minCoords.y, maxCoords.y));
            case 3:
                return new Vector2(Random.Range(minCoords.x, maxCoords.x), maxCoords.y);
            default:
                return new Vector2();
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector2 randomPos = GetRandomPosition();
        return new Vector3(randomPos.x, 3f, randomPos.y);
    }

    private void SpawnEnemy()
    {
        EnemyPoolManager.Instance.SpawnEnemy(GetSpawnPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
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
