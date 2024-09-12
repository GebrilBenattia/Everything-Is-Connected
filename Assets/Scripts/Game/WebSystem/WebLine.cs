using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLine : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // VFX Settings
    [Header("VFX Settings")]
    [SerializeField] private GameObject m_VFX;
    [SerializeField] private Transform m_VFXEndPoint;

    // Damage Settings
    [Header("Collision Settings")]
    [SerializeField] private float m_Thickness;

    // Private Variables
    private Transform m_StartPoint;
    private Transform m_EndPoint;
    private BoxCollider m_Collider;
    private float m_Damage;

    // ###################################### GETTER / SETTER #####################################

    public float damage
    { get { return m_Damage; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Collider = GetComponent<BoxCollider>();
        m_VFX.SetActive(false);
    }

    public void Init(Transform _StartPoint, Transform _EndPoint, float _Damage)
    {
        m_StartPoint = _StartPoint;
        m_EndPoint = _EndPoint;
        m_Damage = _Damage;
    }

    public void SetEndPoint(Transform _EndPoint)
    {
        m_EndPoint = _EndPoint;
        m_VFX.SetActive(true);
    }

    private void Update()
    {
        // Update Web Line Pos
        transform.position = m_StartPoint.position;
        m_VFXEndPoint.position = m_EndPoint.position;

        // Update collider
        float dist = Vector3.Distance(m_StartPoint.position, m_EndPoint.position);
        m_Collider.size = new Vector3(m_Thickness, m_Thickness, dist);
        m_Collider.center = new Vector3(0, 0, dist / 2f);
        Vector3 direction = m_EndPoint.position - m_StartPoint.position;
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
    }
}
