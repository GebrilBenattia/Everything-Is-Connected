using System;
using UnityEngine;
using UnityEngine.Android;

[CreateAssetMenu(fileName = "new NewsData", menuName = "ScriptableObjects/NewsData")]
public class NewsData : ScriptableObject
{
    // ########################################### ENUMS ##########################################

    public enum Theme
    {
        ASTROLOGY,
        RELIGION,
        NASA,
        EINSTEIN,
        PYRAMID,
        ALIEN,
        CLIMATE,
        MAYA
    }

    // ######################################### VARIABLES ########################################

#if UNITY_EDITOR

    // Debug Settings
    [Header("Deabug Settings")]
    [SerializeField] private bool m_GenerateNewGUUID;

#endif

    // Private Variables
    private string m_Guid;

    // Public Variables
    [Header("News Data Settings")]
    public string name;
    public Sprite sprite;
    public Theme theme;

    // ###################################### GETTER / SETTER #####################################

    public string guid
    { get { return m_Guid; } }

    // ######################################### FUNCTIONS ########################################

    public static string ThemeToString(Theme _Theme)
    {
        switch (_Theme) {
            case Theme.ASTROLOGY:   return "ASTROLOGIE";
            case Theme.RELIGION:    return "RELIGION";
            case Theme.NASA:        return "NASA";
            case Theme.EINSTEIN:    return "EINSTEIN";
            case Theme.PYRAMID:     return "PYRAMIDE";
            case Theme.ALIEN:       return "ALIEN";
            case Theme.CLIMATE:     return "CLIMAT";
            case Theme.MAYA:        return "MAYA";
            default:                return string.Empty;
        }
    }

    // Editor Functions
#if UNITY_EDITOR

    private void OnValidate()
    {
        // Check if current guid is empty / null
        if (m_GenerateNewGUUID || String.IsNullOrEmpty(m_Guid)) {

            // Generate new guid
            m_GenerateNewGUUID = false;
            m_Guid = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

#endif
}
