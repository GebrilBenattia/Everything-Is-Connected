using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new EnemyWaveData", menuName = "ScriptableObjects/EnemyWaveData")]
public class EnemyWaveData : ScriptableObject
{
    // ######################################### VARIABLES ########################################

    // Public Variables
    public int tokenCost;
    public GameObject prefab;
}
