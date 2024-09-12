using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JSONLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public static T LOADJSON<T>(string _Filename) where T : class
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(_Filename);

        if (jsonFile != null) 
        {
            return JsonUtility.FromJson<T>(jsonFile.text);
        }

        Debug.LogWarning("JSON file not found");
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
