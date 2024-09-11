using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSegment : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;
    [SerializeField] private float m_LifeTime;

    public float length;

    void Awake()
    {
        Destroy(gameObject, m_LifeTime);
        length = Mathf.Abs(endPoint.localPosition.z - startPoint.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
