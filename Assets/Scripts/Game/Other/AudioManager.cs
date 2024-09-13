using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }


}