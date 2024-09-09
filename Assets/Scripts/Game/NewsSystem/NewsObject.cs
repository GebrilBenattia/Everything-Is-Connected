using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsObject : MonoBehaviour
{
    // VARIABLES
    // ---------
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private NewsData m_NewsData;

    // FUNCTIONS
    // ---------
    private void Awake()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        m_SpriteRenderer.sprite = m_NewsData.sprite;
    }

    public void Init(NewsData _NewsData)
    {
        m_NewsData = _NewsData;
        UpdateSprite();
    }
}
