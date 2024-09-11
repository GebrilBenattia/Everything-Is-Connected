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

        if (theoryHolder != null && theoryHolder.conspiracyTheoryArray != null)
        {
            foreach (var conspiracy in theoryHolder.conspiracyTheoryArray)
            {
                Debug.Log(conspiracy.theme1 + " - " + conspiracy.theme2 + " : " + conspiracy.hoax1 + "\n /////// " + conspiracy.hoax2);
            }
        }
    }

    private void LoadConspiracies()
    {
        theoryHolder = JSONLoader.LOADJSON<ConspiracyTheoryHolder>("ConspiracyTheories");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
