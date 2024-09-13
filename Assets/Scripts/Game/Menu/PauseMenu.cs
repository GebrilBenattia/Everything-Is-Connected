using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool m_IsActive = false;

    [SerializeField] private GameObject m_PauseMenu;
    StudioEventEmitter emitter;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Sprite resumeButtonUnHovered;
    [SerializeField] private Sprite resumeButtonHovered;
    [SerializeField] private Sprite resumeButtonClicked;

    [SerializeField] private Sprite quitButtonUnHovered;
    [SerializeField] private Sprite quitButtonHovered;
    [SerializeField] private Sprite quitButtonClicked;

    [SerializeField] private EventReference hoveredSound;
    [SerializeField] private EventReference clickedSound;

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

    public void OnResumeButtonUnHovered()
    {
        resumeButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //resumeButton.GetComponent<Image>().color = new Vector4(resumeButton.GetComponent<Image>().color.r, resumeButton.GetComponent<Image>().color.g, resumeButton.GetComponent<Image>().color.b, 255f);
    }

    public void OnResumeButtonHovered()
    {
        RuntimeManager.PlayOneShot(hoveredSound);
        //resumeButton.GetComponent<Image>().color = new Vector4(resumeButton.GetComponent<Image>().color.r, resumeButton.GetComponent<Image>().color.g, resumeButton.GetComponent<Image>().color.b, 0f);
        resumeButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnResumeClicked()
    {
        emitter.Stop();
        RuntimeManager.PlayOneShot(clickedSound);
        resumeButton.GetComponent<Image>().sprite = resumeButtonClicked;
        OnResumeButtonUnHovered();
        OnUnPause();
    }

    public void OnQuitClicked()
    {
        emitter.Stop();
        RuntimeManager.PlayOneShot(clickedSound);
        quitButton.GetComponent<Image>().sprite = quitButtonClicked;
        OnQuitButtonUnHovered();
        Application.Quit();
#if DEBUG
        Debug.Log("Quitting");
#endif
    }
    public void OnQuitButtonUnHovered()
    {
        quitButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //quitButton.GetComponent<Image>().color = new Vector4(quitButton.GetComponent<Image>().color.r, quitButton.GetComponent<Image>().color.g, quitButton.GetComponent<Image>().color.b, 255f);
    }

    public void OnQuitButtonHovered()
    {
        RuntimeManager.PlayOneShot(hoveredSound);
        //quitButton.GetComponent<Image>().color = new Vector4(quitButton.GetComponent<Image>().color.r, quitButton.GetComponent<Image>().color.g, quitButton.GetComponent<Image>().color.b, 0f);
        quitButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
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
    }
}
