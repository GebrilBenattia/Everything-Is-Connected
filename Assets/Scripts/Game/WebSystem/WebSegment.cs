using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSegment : MonoBehaviour, IClickableObject
{
    // Start is called before the first frame update
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;
    public float damage;
    [SerializeField] private float m_LifeTime;
    public int webIndex;

    public float length;
    [SerializeField] private int m_ClicksToDelete;
    private int m_Count;

    void Awake()
    {
        Destroy(gameObject, m_LifeTime);
        length = Mathf.Abs(endPoint.localPosition.z - startPoint.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EventOnClick()
    {
       
    }

    public void EventOnClickRelease()
    {
        if (m_Count == m_ClicksToDelete-1)
        {
            WebMaker.instance.OnWebSelected(webIndex);
            m_Count = 0;
        }
        else m_Count++;
    }
}
