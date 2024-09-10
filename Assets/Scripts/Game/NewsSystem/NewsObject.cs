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

    // ######################################### FUNCTIONS ########################################

    
    public void Init(NewsData _NewsData)
    {
        m_NewsData = _NewsData;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        m_SpriteRenderer.sprite = m_NewsData.sprite;
    }

    private void OnCollisionEnter(Collision _Collision)
    {
    }
}
