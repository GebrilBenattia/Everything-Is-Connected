using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CompatibleNewsData", menuName = "ScriptableObjects/CompatibleNewsData")]
public class CompatibleNewsData : ScriptableObject
{
    // ########################################## STRUCTS #########################################

    // Editor Structs
#if UNITY_EDITOR

    [System.Serializable]
    struct CompatibleNewsElementData
    {
        public NewsData newsDataA;
        public NewsData newsDataB;
        public float damage;
    }

#endif

    // ######################################### VARIABLES ########################################

    // Editor Variables
#if UNITY_EDITOR

    [SerializeField] private CompatibleNewsElementData[] compatibleNewsList;

#endif

    // Private Variables
    private Dictionary<Tuple<string, string>, float> m_CompatibleNewsDictionary = new Dictionary<Tuple<string, string>, float>();

    // ###################################### GETTER / SETTER #####################################

    public Dictionary<Tuple<string, string>, float> compatibleNewsDictionary
    { get { return m_CompatibleNewsDictionary; } }

    // ######################################### FUNCTIONS ########################################

    public float Get(string _GuidA, string _GuidB)
    {
        // Search the tuple of these to Guid in dictionary and retrun value
        var searchTuple = Tuple.Create(_GuidA, _GuidB);
        m_CompatibleNewsDictionary.TryGetValue(searchTuple, out float value);

        return value;
    }

    // Editor Functions
#if UNITY_EDITOR

    private void OnValidate()
    {
        // Clear dictionnary
        m_CompatibleNewsDictionary.Clear();

        // Loop on compatible News List
        for (int i = 0; i < compatibleNewsList.Length; i++) {

            // Variables
            var tuple = Tuple.Create(compatibleNewsList[i].newsDataA.guid, compatibleNewsList[i].newsDataB.guid);
            var damage = compatibleNewsList[i].damage;

            // Add new element to dictionary
            m_CompatibleNewsDictionary.Add(tuple, damage);
        }
    }

#endif
}
