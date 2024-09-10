using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebMaker : MonoBehaviour
{
    [SerializeField] private float m_RayDistance;
    [SerializeField] private string m_TagToLink;
    [SerializeField] private GameObject m_StringSegmentToSpawn;
    [SerializeField] private float m_segmentLength;
    [SerializeField] private float m_SegmentSpawnDelay;
    [SerializeField] private float m_NewsMoveRange;
    private Camera m_Camera;
    private System.Action m_DoAction;
    private GameObject m_SelectedNode;
    private GameObject m_StartPoint;
    private GameObject m_EndPoint;
    private Vector3 m_CurrentStringStartPoint;
    private int m_SegmentsToSpawn;
    private int m_SegmentSpawned;
    private Vector3 m_LastNewsPos;
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
        if (CheckForNode())
        {
            
            /*if (Input.GetMouseButtonUp(0)) SetModeSowing();
            else*/ if (Input.GetMouseButtonDown(0)) SetModeNewsMove();
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
                m_SegmentsToSpawn = (int)(Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position) / m_segmentLength);

                AddSegment();
            }
            SetModeDefault();
        }
    }

    private void SetModeNewsMove()
    {
        m_LastNewsPos = m_SelectedNode.transform.position;
        m_DoAction = DoActionNewsMove;
    }

    private void DoActionNewsMove()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_LastNewsPos = Vector3.zero;
            SetModeSowing();
            return;
        }
        Ray lRay = m_Camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(lRay, out RaycastHit hitInfo, m_RayDistance);        
        Vector3 mousePos = hitInfo.point;
        float ClampedX = Mathf.Clamp(mousePos.x, m_LastNewsPos.x - m_NewsMoveRange, m_LastNewsPos.x + m_NewsMoveRange);
        float ClampedZ = Mathf.Clamp(mousePos.z, m_LastNewsPos.z - m_NewsMoveRange, m_LastNewsPos.z + m_NewsMoveRange);
        Debug.Log(ClampedX);
        Debug.Log(ClampedZ);
        Vector3 velocity = new Vector3(ClampedX, m_SelectedNode.transform.position.y, ClampedZ);

        Debug.Log(velocity.x);
        m_SelectedNode.transform.position = velocity;
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
    private void AddSegment()
    {
        m_SegmentSpawned++;
        m_SegmentsToSpawn--;
        Quaternion rotation = Quaternion.LookRotation(m_EndPoint.transform.position - m_StartPoint.transform.position);
        GameObject segment = Instantiate(m_StringSegmentToSpawn, m_CurrentStringStartPoint, rotation);
        float distance = Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position);
        float ratio = (segment.GetComponent<WebSegment>().length / distance) * m_SegmentSpawned;
        Vector3 segmentPos = Vector3.Lerp(m_StartPoint.transform.position, m_EndPoint.transform.position, ratio);
        segment.transform.position = segmentPos;
        m_CurrentStringStartPoint = segment.GetComponent<WebSegment>().endPoint.position;
        if (m_SegmentsToSpawn == 0)
        {
            ClearValues();
        }
        else Invoke(nameof(AddSegment), m_SegmentSpawnDelay);
    }

    private void ClearValues()
    {
        Debug.Log("cleared");
        m_SegmentSpawned = 0;
        m_SelectedNode = null;
        m_StartPoint = null;
        m_EndPoint = null;
    }
}

