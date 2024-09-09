using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new NewsData", menuName = "ScriptableObjects/NewsData")]
public class NewsData : ScriptableObject
{
    // VARIABLES
    // ---------
    private string m_Guid;
    public string name;
    public Sprite sprite;

    // GETTER / SETTER
    // ---------------
    public string guid
    { get { return m_Guid; } }

    // FUNCTIONS
    // ---------
#if UNITY_EDITOR
    private void OnValidate()
    {
        // Check if current guid is empty / null
        if (String.IsNullOrEmpty(m_Guid)) {

            // Generate new guid
            m_Guid = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
