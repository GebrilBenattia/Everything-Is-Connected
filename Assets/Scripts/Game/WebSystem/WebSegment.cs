using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSegment : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform m_StartPoint;
    [SerializeField] public Transform m_EndPoint;

    public float length;

    void Awake()
    {
        length = Mathf.Abs(m_EndPoint.localPosition.z - m_StartPoint.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
