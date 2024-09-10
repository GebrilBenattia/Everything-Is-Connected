using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSegment : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;

    public float length;

    void Awake()
    {
        length = Mathf.Abs(endPoint.localPosition.z - startPoint.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
