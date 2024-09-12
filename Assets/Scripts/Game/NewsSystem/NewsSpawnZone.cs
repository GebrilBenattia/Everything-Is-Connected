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
    [SerializeField] private bool m_ShowArea;
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

    public void Init(Vector3 _Position, Vector3 _Size)
    {
        m_AreaSize = _Size;
        transform.position = _Position;
        m_BoxCollider.center = Vector3.zero;
        m_BoxCollider.size = m_AreaSize;
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
        if (m_ShowArea) {
            Gizmos.color = m_AreaColor;
            Gizmos.DrawCube(transform.position, m_AreaSize);
        }
    }

#endif
}
