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

    // Game Settings
    [Header("Game Settings")]
    [SerializeField] public float life;

    // Conspiracy system objects
    [SerializeField] public NewsConspiracyHolder newsConspiracyHolder;
    private List<string> m_loadedThemes = new List<string>();
    private const int MAX_THEMES = 2;

    // SmartBomb variables
    [SerializeField] private int m_BubbleAmountBeforeSmartBomb;
    private int m_BubbleAmount;
    [SerializeField] private int m_DamageForBubble;

    public NewsLinkData newsLinkData;
    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
    }

    /*public void LoadTheme(string _ThemeToLoad)
    {
        m_loadedThemes.Add(_ThemeToLoad);
        if (m_loadedThemes.Count == MAX_THEMES)
        {
            m_BubbleAmount++;
            CreateBubble(m_loadedThemes[0], m_loadedThemes[1]);           
            m_loadedThemes.Clear();
        }
    }*/

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
        
        Debug.Log(m_BubbleAmount);
    }

    private void SmartBomb()
    {
        Debug.Log("KILL THEM ALL!!!!!!!!!!!!!!!!");
    }
}
