using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsObject : MonoBehaviour, IClickableObject
{
    // ######################################### VARIABLES ########################################

#if UNITY_EDITOR

    // Debug Settings
    [Header("Debug Settings")]
    [SerializeField] private bool m_ShowMoveRadius;

#endif

    // Object Settings
    [Header("Object Settings")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_AttractiveSpeed;
    [SerializeField] private float m_MoveRadius;
    [SerializeField] private float m_NormalResistDampFactor;
    [SerializeField] private float m_DragResistDampFactor;

    // Private Variables
    private NewsData m_NewsData;
    private Rigidbody m_Rigidbody;
    private Vector3 m_InitPos;
    private bool m_IsSelected = false;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void EventOnClick()
    {
        m_IsSelected = true;
    }

    public void EventOnClickRelease()
    {
        m_IsSelected = false;
    }

    public void Init(NewsData _NewsData)
    {
        m_NewsData = _NewsData;
        m_InitPos = transform.position;
        SetSprite();
        StartCoroutine(SpawnScaleEffect());
    }

    private IEnumerator SpawnScaleEffect()
    {
        // Variables
        float speed = 4f;
        float t = 0f;

        // Update scale
        while (t < 1) {

            transform.localScale = Vector3.one * t;
            t += Time.deltaTime * speed;

            yield return null;
        }

        // Set scale to Vector3 One
        transform.localScale = Vector3.one;
    }

    private void SetSprite()
    {
        m_SpriteRenderer.sprite = m_NewsData.sprite;
    }

    private void MoveToCursor()
    {
        // Variables
        Vector2 screenPos = GameplayManager.Instance.camera.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition;
        Vector2 direction =  mousePos - screenPos;
        direction /= direction.magnitude;

        // Update velocity
        Vector2 velocity = direction * Time.fixedDeltaTime * m_MoveSpeed;
        m_Rigidbody.velocity += new Vector3(velocity.x, 0, velocity.y);
    }

    private void MovementsLimit()
    {
        // Check if the distance between current pos and init pos is >= moveRadius
        if (Vector3.Distance(transform.position, m_InitPos) >= m_MoveRadius) {

            // Change resist damping factor
            if (!m_IsSelected) m_Rigidbody.drag = m_DragResistDampFactor;

            // Update velocity to recenter the newsObject
            Vector3 direction = m_InitPos - transform.position;
            direction /= direction.magnitude;
            m_Rigidbody.velocity += direction * Time.fixedDeltaTime * m_AttractiveSpeed;
        }
        // Change resist damping factor
        else m_Rigidbody.drag = m_NormalResistDampFactor;
    }

    private void FixedUpdate()
    {
        if (m_IsSelected) MoveToCursor();
        MovementsLimit();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (m_ShowMoveRadius) {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawSphere(m_InitPos, m_MoveRadius);
        }
    }

#endif
}
