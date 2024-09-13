using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    StudioEventEmitter emitter;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Sprite restartButtonUnHovered;
    [SerializeField] private Sprite restartButtonHovered;
    [SerializeField] private Sprite restartButtonClicked;

    [SerializeField] private Sprite quitButtonUnHovered;
    [SerializeField] private Sprite quitButtonHovered;
    [SerializeField] private Sprite quitButtonClicked;

    [SerializeField] private EventReference hoveredSound;
    [SerializeField] private EventReference clickedSound;


    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
        emitter.Play();
    }

    public void OnRestartButtonUnHovered()
    {
        restartButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //restartButton.GetComponent<Image>().color = new Vector4(restartButton.GetComponent<Image>().color.r, restartButton.GetComponent<Image>().color.g, restartButton.GetComponent<Image>().color.b, 255f);
    }

    public void OnRestartButtonHovered()
    {
        RuntimeManager.PlayOneShot(hoveredSound);
        //restartButton.GetComponent<Image>().color = new Vector4(restartButton.GetComponent<Image>().color.r, restartButton.GetComponent<Image>().color.g, restartButton.GetComponent<Image>().color.b, 0f);
        restartButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnRestartClicked()
    {
        emitter.Stop();
        RuntimeManager.PlayOneShot(clickedSound);
        restartButton.GetComponent<Image>().sprite = restartButtonClicked;
        OnRestartButtonUnHovered();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
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

    }
}
