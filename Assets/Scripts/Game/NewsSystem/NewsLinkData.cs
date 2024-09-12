using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new NewsLinkData", menuName = "ScriptableObjects/NewsLinkData")]
public class NewsLinkData : ScriptableObject
{
    // ########################################## STRUCTS #########################################

    // Editor Structs
#if UNITY_EDITOR

    [System.Serializable]
    private struct NewsLinkElementData
    {
        public NewsData newsDataA;
        public NewsData newsDataB;
        public float damage;
        public string _Text;
    }

#endif

    // ######################################### VARIABLES ########################################

    // Editor Variables
#if UNITY_EDITOR

    [SerializeField] private NewsLinkElementData[] compatibleNewsList;

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

    // Editor Functionsx
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
