using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsObject : MonoBehaviour, IClickableObject
{
    // ########################################### ENUMS ##########################################

    private enum InteractionType
    {
        None,
        SimpleClick,
        StartMoving
    }
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
    [SerializeField] private float m_ConnectionTime;

    // Interaction Settings
    [Header("Interaction Settings")]
    [SerializeField] private float m_PressTimeToMove;

    // Private Variables
    private NewsData m_NewsData;
    private Rigidbody m_Rigidbody;
    private Vector3 m_InitPos;
    private bool m_CanMoveToCursor = false;
    private bool m_IsLeftButtonDown = false;
    private InteractionType m_InteractionType = InteractionType.None;

    // Public variables
    public bool connected;
    public string newsType;

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private IEnumerator LeftButtonPressUpdate()
    {
        // Variables
        float pressTime = 0f;

        // Update press time
        while (m_IsLeftButtonDown && pressTime < m_PressTimeToMove) {
            pressTime += Time.deltaTime;
            yield return null;
        }

        // If press time exceed min press time to move: start moving
        if (m_IsLeftButtonDown && pressTime >= m_PressTimeToMove) m_CanMoveToCursor = true;
        // Else add newsObject as link node
        else WebManager.instance.AddNewsObjectAsLinkNode(this);
    }

    public void EventOnLeftButtonDown(RaycastHit _HitInfo)
    {
        m_IsLeftButtonDown = true;
        StartCoroutine(LeftButtonPressUpdate());
        GameplayManager.Instance.LoadTheme(m_NewsData.name);
    }

    public void EventOnLeftButtonUp()
    {
        m_CanMoveToCursor = false;
        m_IsLeftButtonDown = false;
    }

    public void OnConneCtion()
    {
        connected = true;
        Invoke(nameof(Disconnect), m_ConnectionTime);
    }

    private void Disconnect()
    {
        connected = false;
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
            if (!m_CanMoveToCursor) m_Rigidbody.drag = m_DragResistDampFactor;

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
        if (m_CanMoveToCursor) MoveToCursor();
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
