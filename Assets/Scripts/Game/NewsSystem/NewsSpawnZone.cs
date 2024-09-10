using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsSpawnZone : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Spawn Settings
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 m_AreaSize;

    // Private Variables
    private BoxCollider m_BoxCollider;
    private int m_NewsCount;

    // Editor Variables
#if UNITY_EDITOR

    [Header("Debug Settings")]
    [SerializeField] private Color m_AreaColor;

#endif

    // ###################################### GETTER / SETTER #####################################

    public int newsCount
    { get { return m_NewsCount; }  }
    public Vector3 areaSize
    { get { return m_AreaSize; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider _Other)
    {
        if (_Other.CompareTag("NewsObject")) ++m_NewsCount;
    }

    private void OnTriggerExit(Collider _Other)
    {
        if (_Other.CompareTag("NewsObject")) --m_NewsCount;
    }

    // Editor Functions
#if UNITY_EDITOR

    private void OnValidate()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.center = Vector3.zero;
        m_BoxCollider.size = m_AreaSize;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw debug spawn area
        Gizmos.color = m_AreaColor;
        Gizmos.DrawCube(transform.position, m_AreaSize);
    }

#endif
}
