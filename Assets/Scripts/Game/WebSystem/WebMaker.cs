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
        Debug.Log("start");
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
                Debug.Log(m_EndPoint.name);
                m_CurrentStringStartPoint = m_StartPoint.transform.position;
                for (int i = 1; i < 4; i++)
                {
                    AddSegment(i);
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

    private void AddSegment(float pIndex)
    {
        Quaternion lRotation = Quaternion.LookRotation(m_EndPoint.transform.position - m_StartPoint.transform.position);
        GameObject segment = Instantiate(m_StringSegmentToSpawn, m_CurrentStringStartPoint, lRotation);
        //Vector3 lStartPosDelay = segment.GetComponent<WebSegment>().m_StartPoint.localPosition;
        //float lDistance = Mathf.Sqrt(Mathf.Pow(m_EndPoint.transform.position.x, 2) + Mathf.Pow(m_EndPoint.transform.position.z, 2));
        float lDistance = Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position);
        float lRatio = (segment.GetComponent<WebSegment>().length / lDistance) * pIndex;
        Debug.Log(lDistance);
        Debug.Log(lRatio);
        Vector3 truc = Vector3.Lerp(m_StartPoint.transform.position, m_EndPoint.transform.position, lRatio);
        segment.transform.position = truc;
        //Debug.Log(lStartPosDelay);
        //segment.transform.position += new Vector3(lStartPosDelay.x, 0, lStartPosDelay.z);
        m_CurrentStringStartPoint = segment.GetComponent<WebSegment>().m_EndPoint.position;
    }
}

