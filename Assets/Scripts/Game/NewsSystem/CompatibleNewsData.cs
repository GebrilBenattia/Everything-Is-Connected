using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "new CompatibleNewsData", menuName = "ScriptableObjects/CompatibleNewsData")]
public class CompatibleNewsData : ScriptableObject
{
    // STRUCTS
    // -------
#if UNITY_EDITOR
    [System.Serializable]
    struct CompatibleNewsElementData
    {
        public NewsData newsDataA;
        public NewsData newsDataB;
        public float damage;
    }
#endif

    // VARIABLES
    // ---------
#if UNITY_EDITOR
    [SerializeField] private List<CompatibleNewsElementData> compatibleNewsList = new List<CompatibleNewsElementData>();
#endif
    public Dictionary<Tuple<string, string>, float> compatibleNewsDictionary = new Dictionary<Tuple<string, string>, float>();

    // FUNCTIONS
    // ---------
#if UNITY_EDITOR
    private void OnValidate()
    {
        // Clear dictionnary
        compatibleNewsDictionary.Clear();

        // Loop on compatible News List
        for (int i = 0; i < compatibleNewsList.Count; i++) {

            // Variables
            var tuple = Tuple.Create(compatibleNewsList[i].newsDataA.guid, compatibleNewsList[i].newsDataB.guid);
            var damage = compatibleNewsList[i].damage;

            // Add new element to dictionary
            compatibleNewsDictionary.Add(tuple, damage);
        }
    }
#endif

    public float Get(string _GuidA, string _GuidB)
    {
        // Search the tuple of these to Guid in dictionary and retrun value
        var searchTuple = Tuple.Create(_GuidA, _GuidB);
        compatibleNewsDictionary.TryGetValue(searchTuple, out float value);

        return value;
    }
}
