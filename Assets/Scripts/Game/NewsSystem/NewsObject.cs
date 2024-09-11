using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsObject : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Object Settings
    [Header("Object Settings")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    // Private Variables
    private NewsData m_NewsData;
    public Vector3 initialPos; //ML
    public bool connected;//ML
    [SerializeField] private float m_ConnectedTime;

    // ######################################### FUNCTIONS ########################################

    private void Start()
    {
        initialPos = transform.position;
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

    public void Init(NewsData _NewsData)
    {
        m_NewsData = _NewsData;
        UpdateSprite();
        StartCoroutine(SpawnScaleEffect());
    }

    private void UpdateSprite()
    {
        m_SpriteRenderer.sprite = m_NewsData.sprite;
    }

    public void OnConneCtion()
    {
        connected = true;
        Invoke(nameof(Disconnect), m_ConnectedTime);
    }

    private void Disconnect()
    {
        connected = false;
    }
}
