using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConspiracyTheory
{
    public string theme1;
    public string theme2;
    public string hoax1;
    public string hoax2;
}

[System.Serializable]
public class ConspiracyTheoryHolder
{
    public ConspiracyTheory[] conspiracyTheoryArray;
}

public class NewsConspiracyHolder : MonoBehaviour
{
    public ConspiracyTheoryHolder theoryHolder;

    // Start is called before the first frame update
    void Start()
    {
        LoadConspiracies();
    }

    private void LoadConspiracies()
    {
        theoryHolder = JSONLoader.LOADJSON<ConspiracyTheoryHolder>("ConspiracyTheories");
    }

    // Function to get a random hoax for a given pair of themes
    public string GetRandomHoax(string _Theme1, string _Theme2)
    {
        if (theoryHolder == null || theoryHolder.conspiracyTheoryArray == null) 
            return null;

        foreach (var conspiracy in theoryHolder.conspiracyTheoryArray)
        {
            // Check if the themes match (ignoring order)
            if ((conspiracy.theme1 == _Theme1 && conspiracy.theme2 == _Theme2) || (conspiracy.theme1 == _Theme2 && conspiracy.theme2 == _Theme1))
            {
                // Randomly choose between hoax1 and hoax2
                int randomIndex = Random.Range(0, 2);
                return randomIndex == 0 ? conspiracy.hoax1 : conspiracy.hoax2;
            }
        }

        return null; // No matching conspiracy found
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Debug.Log(GetRandomHoax("MAYA", "ASTROLOGIE"));
    }
}
