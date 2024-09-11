using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static GameplayManager m_Instance;
    public static GameplayManager Instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

    // Object References
    [Header("Object References")]
    [SerializeField] public Camera camera;

    // Game Settings
    [Header("Game Settings")]
    [SerializeField] public float life;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
    }
}
