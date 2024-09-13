using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLineDestroySFX : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Private Variables
    private StudioEventEmitter m_EventEmitter;

    // ######################################### FUNCTIONS ########################################

    private IEnumerator PlaySFX()
    {
        m_EventEmitter.Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void Awake()
    {
        StartCoroutine(PlaySFX());
    }
}
