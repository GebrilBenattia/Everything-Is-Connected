using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebMaker : MonoBehaviour
{
    [SerializeField] private float m_RayDistance;
    [SerializeField] private string m_TagToLink;
    [SerializeField] private GameObject m_StringSegmentToSpawn;
    [SerializeField] private float m_segmentLength;
    private Camera m_Camera;
    private System.Action m_DoAction;
    private GameObject m_SelectedNode;
    private GameObject m_StartPoint;
    private GameObject m_EndPoint;
    private Vector3 m_CurrentStringStartPoint;

    void Start()
    {
        m_Camera = Camera.main;
        SetModeDefault();
    }

    void Update()
    {
        m_DoAction();
    }

    //StateMachine
    private void SetModeDefault()
    {
        m_DoAction = DoActionDefault;
    }

    private void DoActionDefault()
    {
        if (CheckForNode() && Input.GetMouseButton(0))
        {
            SetModeSowing();
        }
    }

    private void SetModeSowing()
    {
        m_StartPoint = m_SelectedNode;
        m_DoAction = DoActionSowing;
    }

    private void DoActionSowing()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (CheckForNode())
            {
                m_EndPoint = m_SelectedNode;
                Debug.Log(m_EndPoint.transform.position);
                Debug.Log(m_EndPoint.name);
                m_CurrentStringStartPoint = m_StartPoint.transform.position;
                int maxSegments = (int)(Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position) / m_segmentLength);
                Debug.Log(maxSegments);
                for (int i = 1; i < maxSegments+1; i++)
                {
                    AddSegment(i);
                }
            }
            SetModeDefault();
            ClearValues();
        }

    }

    //Check for NewsObject with raycast
    private bool CheckForNode()
    {
        bool lRaycast = false;

        Ray lRay = m_Camera.ScreenPointToRay(Input.mousePosition);

        bool lRayCastHit = Physics.Raycast(lRay, out RaycastHit hitInfo, m_RayDistance);

        if (lRayCastHit && hitInfo.collider.gameObject.CompareTag(m_TagToLink))
        {
            m_SelectedNode = hitInfo.collider.gameObject;
            lRaycast = true;
        };

        return lRaycast;
    }

    //Add a web segment
    private void AddSegment(float _Index)
    {
        Quaternion rotation = Quaternion.LookRotation(m_EndPoint.transform.position - m_StartPoint.transform.position);
        GameObject segment = Instantiate(m_StringSegmentToSpawn, m_CurrentStringStartPoint, rotation);
        float distance = Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position);
        float ratio = (segment.GetComponent<WebSegment>().length / distance) * _Index;
        Vector3 segmentPos = Vector3.Lerp(m_StartPoint.transform.position, m_EndPoint.transform.position, ratio);
        segment.transform.position = segmentPos;
        m_CurrentStringStartPoint = segment.GetComponent<WebSegment>().endPoint.position;
    }

    private void ClearValues()
    {
        m_SelectedNode = null;
        m_StartPoint = null;
        m_EndPoint = null;
    }
}

