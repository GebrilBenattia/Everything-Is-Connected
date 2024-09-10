using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebMaker : MonoBehaviour
{
    [SerializeField] private float m_RayDistance;
    [SerializeField] private string m_TagToLink;
    [SerializeField] private GameObject m_StringSegmentToSpawn;
    private Camera m_Camera;
    private System.Action m_DoAction;
    private GameObject m_SelectedNode;
    private GameObject m_StartPoint;
    private GameObject m_EndPoint;
    private Vector3 m_Direction;
    private Vector3 m_CurrentStringStartPoint;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        SetModeDefault();
    }

    // Update is called once per frame
    void Update()
    {
        m_DoAction();
    }

    private void SetModeDefault()
    {
        m_DoAction = DoActionDefault;
    }

    private void DoActionDefault()
    {
        if (CheckForNode() && Input.GetMouseButton(0))
        {
            SetModeSowing();
        };
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
                m_CurrentStringStartPoint = m_StartPoint.transform.position;
                for (int i = 0; i < 5; i++)
                {
                    AddSegment();
                }

                //m_Direction = m_EndPoint - m_StartPoint;
            }
            else
            {
                SetModeDefault();
            }
            /*m_StartPoint = Vector3.zero;
            m_EndPoint = Vector3.zero;*/
            m_SelectedNode = null;
        }

    }

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

    private void AddSegment()
    {
        Quaternion lRotation = Quaternion.LookRotation(m_EndPoint.transform.position);
        GameObject segment = Instantiate(m_StringSegmentToSpawn, m_CurrentStringStartPoint, lRotation);
        segment.transform.position += segment.GetComponent<WebSegment>().m_StartPoint.position;
        m_CurrentStringStartPoint = segment.GetComponent<WebSegment>().m_EndPoint.position;
    }
}

