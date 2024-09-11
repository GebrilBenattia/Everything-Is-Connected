using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsObject : MonoBehaviour, IClickableObject
{
    // ######################################### VARIABLES ########################################

    // Object Settings
    [Header("Object Settings")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_ConnectionTime;

    // Private Variables
    private NewsData m_NewsData;
    private Rigidbody m_Rigidbody;
    private bool m_IsSelected = false;

    // Public variables
    public bool connected;
    public Vector3 initialPos;

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

        // Update Rigidbody Velocity
        Vector2 velocity = direction * Time.fixedDeltaTime * m_MoveSpeed;
        m_Rigidbody.velocity += new Vector3(velocity.x, 0, velocity.y);
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

    private void FixedUpdate()
    {
        if (m_IsSelected) MoveToCursor();
    }
}
