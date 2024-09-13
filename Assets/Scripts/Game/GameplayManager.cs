using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static GameplayManager m_Instance;
    public static GameplayManager Instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

    // Object References
    [Header("Object References")]
    [SerializeField] public Camera camera;
    [SerializeField] private GameObject m_GameOverMenu;

    // Game Settings
    [Header("Game Settings")]
    [SerializeField] private float m_MaxLife;

    // Public Variables
    [NonSerialized] public float life;

    // Conspiracy system objects
    [SerializeField] public NewsConspiracyHolder newsConspiracyHolder;
    private List<string> m_loadedThemes = new List<string>();
    private const int MAX_THEMES = 2;

    // SmartBomb variables
    [SerializeField] private int m_BubbleAmountBeforeSmartBomb;
    private int m_BubbleAmount;
    [SerializeField] private int m_DamageForBubble;

    public NewsLinkData newsLinkData;
    private StudioEventEmitter m_EventEmitter;

    // ###################################### GETTER / SETTER #####################################

    public float maxLife
    { get { return m_MaxLife; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
        life = m_MaxLife;
        m_EventEmitter = GetComponent<StudioEventEmitter>();
    }

    public void PLayDamageSound()
    {
        m_EventEmitter.Play();
    }

    public void GameOver()
    {
        m_GameOverMenu.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void CheckNewsLink(string _Theme1, string _Theme2, float _Damage)
    {
        string conspiracy = newsConspiracyHolder.GetRandomHoax(_Theme1, _Theme2);
        if (_Damage >= m_DamageForBubble)
        {
            m_BubbleAmount++;
            if (m_BubbleAmount == m_BubbleAmountBeforeSmartBomb)
            {
                SmartBomb();
                m_BubbleAmount = 0;
                UIManager.instance.ResetBubbles();
            }
            else
            {
                UIManager.instance.UpdateBubbles(m_BubbleAmount - 1, conspiracy);
            }
        }
        else UIManager.instance.StartHeadlines(conspiracy);
    }

    private void SmartBomb()
    {
        Debug.Log("KILL THEM ALL!!!!!!!!!!!!!!!!");
    }
}
