using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WebManager;

public class GameManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static GameManager m_Instance;
    public static GameManager instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

    // Game Manager Settings
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D m_CursorTexture;
    [SerializeField] private Vector2 m_CursorOffset;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        // Check if instance is null
        if (m_Instance == null) {

            // Create the instance (Dont destroy on load)
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Destroy others GameManager
        else {
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        Cursor.SetCursor(m_CursorTexture, m_CursorOffset, CursorMode.ForceSoftware);
    }
}
