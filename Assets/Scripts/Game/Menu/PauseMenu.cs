using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool m_IsActive = false;

    [SerializeField] private GameObject m_PauseMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnPause()
    {
        Time.timeScale = 0f;
        m_PauseMenu.active = true;
    }

    public void OnUnPause()
    {
        Time.timeScale = 1f;
        m_PauseMenu.active = false;
    }

    public void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Quitting");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_IsActive = true;
                OnPause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_IsActive = false;
                OnUnPause();
            }
        }

        Debug.Log(Time.timeScale);
    }
}
