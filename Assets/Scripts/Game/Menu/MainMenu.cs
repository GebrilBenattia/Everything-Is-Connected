using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    StudioEventEmitter emitter;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Sprite playButtonUnHovered;
    [SerializeField] private Sprite playButtonHovered;
    [SerializeField] private Sprite playButtonClicked;

    [SerializeField] private Sprite quitButtonUnHovered;
    [SerializeField] private Sprite quitButtonHovered;
    [SerializeField] private Sprite quitButtonClicked;


    // Start is called before the first frame update
    void Start()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/MUSIC/ui_loop");
        emitter = GetComponent<StudioEventEmitter>();
        emitter.Play();
    }

    public void OnPlayButtonUnHovered()
    {
        playButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        playButton.GetComponent<Image>().color = new Vector4(playButton.GetComponent<Image>().color.r, playButton.GetComponent<Image>().color.g, playButton.GetComponent<Image>().color.b, 255f);
    }

    public void OnPlayButtonHovered()
    {
        playButton.GetComponent<Image>().color = new Vector4(playButton.GetComponent<Image>().color.r, playButton.GetComponent<Image>().color.g, playButton.GetComponent<Image>().color.b, 0f);
        playButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPlayClicked()
    {
        playButton.GetComponent<Image>().sprite = playButtonClicked;
        OnPlayButtonUnHovered();
        emitter.Stop();
        SceneManager.LoadScene("Game");
    }

    public void OnQuitClicked()
    {
        quitButton.GetComponent<Image>().sprite = quitButtonClicked;
        OnQuitButtonUnHovered();
        emitter.Stop();
        Application.Quit();
#if DEBUG
        Debug.Log("Quitting");
#endif
    }
    public void OnQuitButtonUnHovered()
    {
        quitButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        quitButton.GetComponent<Image>().color = new Vector4(quitButton.GetComponent<Image>().color.r, quitButton.GetComponent<Image>().color.g, quitButton.GetComponent<Image>().color.b, 255f);
    }

    public void OnQuitButtonHovered()
    {
        quitButton.GetComponent<Image>().color = new Vector4(quitButton.GetComponent<Image>().color.r, quitButton.GetComponent<Image>().color.g, quitButton.GetComponent<Image>().color.b, 0f);
        quitButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
