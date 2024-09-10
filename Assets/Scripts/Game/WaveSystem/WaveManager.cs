using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Tooltip("Current Wave Number")]
    [SerializeField] private uint currentWave;

    [Tooltip("Maximum number of tokens for the currentWave")]
    [SerializeField] private uint spawnToken;

    [Tooltip("Increment that will be applied to the spawn token after each waves")]
    [SerializeField] private uint tokenIncrement;

    [Tooltip("")]
    [SerializeField] private uint timeBetweenWaves;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
