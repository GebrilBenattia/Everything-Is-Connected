using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{

    // ######################################### VARIABLES ########################################

    // Spider Settings
    [Header("Spider Setting")]
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_AngularSpeed;
    [SerializeField] private float m_DetectionRadius;
    [SerializeField] private Animator m_Animator;

    // Private Variables
    private List<WebManager.LinkData> m_LinkDataList = new List<WebManager.LinkData>();
    private NewsObject m_TargetNewsObject = null;
    private bool m_MoveToTarget = false;
    private Vector3 m_TargetPos;

    // ###################################### GETTER / SETTER #####################################

    public NewsObject targetNewsObject
    { get { return m_TargetNewsObject; } }

    public WebManager.LinkData currentLinkData
    { get { return m_LinkDataList.Count == 0 ? new WebManager.LinkData() : m_LinkDataList[0]; } }

    public int currentLinkCount
    {  get { return m_LinkDataList.Count;} }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Animator.enabled = false;
    }

    public bool AreAlreadyLinked(WebManager.LinkData _LinkData)
    {
        return !m_LinkDataList.Contains(_LinkData);
    }

    public void StopCurrentLink()
    {
        if (m_LinkDataList.Count > 0) m_LinkDataList.RemoveAt(0);
        if (m_LinkDataList.Count == 0) m_TargetNewsObject = null;
    }

    public void RemoveAllLinkFromNewsObject(NewsObject _NewsObject)
    {
        // Loop on each link data 
        for (int i = 0; i < m_LinkDataList.Count;) {

            // Remove news node element if contain _NewsObject
            if (m_LinkDataList[i].linkNewsNodes.startNode == _NewsObject ||
                m_LinkDataList[i].linkNewsNodes.endNode == _NewsObject)
                m_LinkDataList.RemoveAt(i);
            else ++i;
        }

        // Update m_TargetNewsObject value
        if (m_LinkDataList.Count == 0) m_TargetNewsObject = null;
        else if (m_TargetNewsObject == _NewsObject) m_TargetNewsObject = m_LinkDataList[0].linkNewsNodes.startNode;

        if (m_TargetNewsObject == null) m_Animator.enabled = false;
    }

    public void SetTargetPos(Vector3 _TargetPos)
    {
        m_MoveToTarget = true;
        m_TargetPos = _TargetPos;
        m_Animator.enabled = true;
    }

    public void LinkNews(WebManager.LinkData _LinkData)
    {
        if (m_LinkDataList.Count == 0) m_TargetNewsObject = _LinkData.linkNewsNodes.startNode;
        m_LinkDataList.Add(_LinkData);

        m_Animator.enabled = true;
    }

    private void UpdateMoveTo(Vector3 _TargetPos)
    {
        // Calculate direction
        Vector3 direction = _TargetPos - transform.position;
        direction.y = 0;
        direction /= direction.magnitude;

        // Update position
        transform.position += direction * Time.deltaTime * m_Speed;

        // Update rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * m_AngularSpeed);
    }

    private void MoveToTargetNewsObject()
    {
        // Move transform to reach target newsObject
        if (Vector3.Distance(m_TargetNewsObject.transform.position, transform.position) > m_DetectionRadius)
            UpdateMoveTo(m_TargetNewsObject.transform.position);

        // The spider reach the current target -> change target
        else if (m_TargetNewsObject != null && m_LinkDataList.Count > 0)
        {

            // If was firstNode, change target to endNode
            if (m_LinkDataList[0].linkNewsNodes.startNode == m_TargetNewsObject) {

                m_LinkDataList[0].webLine.SetEndPoint(transform);
                m_TargetNewsObject.EventOnLink(m_LinkDataList[0].linkNewsNodes.endNode);
                m_TargetNewsObject = m_LinkDataList[0].linkNewsNodes.endNode;
            }

            // Else change target to next linkData or set target to null
            else {
                m_LinkDataList[0].webLine.SetEndPoint(m_LinkDataList[0].linkNewsNodes.endNode.transform);
                m_TargetNewsObject.EventOnLink(m_LinkDataList[0].linkNewsNodes.startNode);
                m_LinkDataList.RemoveAt(0);
                m_TargetNewsObject = m_LinkDataList.Count > 0 ? m_LinkDataList[0].linkNewsNodes.startNode : null;

                if (m_TargetNewsObject == null) m_Animator.enabled = false;
            }
        }
    }

    private void MoveToTargetPos()
    {
        // Move transform to target pos
        if (Vector3.Distance(m_TargetPos, transform.position) > m_DetectionRadius)
            UpdateMoveTo(m_TargetPos);

        // The spider reach the current target -> stop to move
        else {
            m_Animator.enabled = false;
            m_MoveToTarget = false;
        }
    }

    private void Update()
    {
        if (m_TargetNewsObject != null) MoveToTargetNewsObject();
        else if (m_MoveToTarget) MoveToTargetPos();
    }
}
