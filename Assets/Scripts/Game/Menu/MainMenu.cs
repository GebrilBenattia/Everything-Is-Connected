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

    [SerializeField] private EventReference hoveredSound;
    [SerializeField] private EventReference clickedSound;


    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
        emitter.Play();
    }

    public void OnPlayButtonUnHovered()
    {
        playButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        //playButton.GetComponent<Image>().color = new Vector4(playButton.GetComponent<Image>().color.r, playButton.GetComponent<Image>().color.g, playButton.GetComponent<Image>().color.b, 255f);
    }

    public void OnPlayButtonHovered()
    {
        RuntimeManager.PlayOneShot(hoveredSound);
        //playButton.GetComponent<Image>().color = new Vector4(playButton.GetComponent<Image>().color.r, playButton.GetComponent<Image>().color.g, playButton.GetComponent<Image>().color.b, 0f);
        playButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPlayClicked()
    {
        RuntimeManager.PlayOneShot(clickedSound);
        playButton.GetComponent<Image>().sprite = playButtonClicked;
        OnPlayButtonUnHovered();
        emitter.Stop();
        SceneManager.LoadScene("Game");
    }

    public void OnQuitClicked()
    {
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
