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

    // Private Variables
    private List<WebManager.LinkData> m_LinkDataList = new List<WebManager.LinkData>();
    private NewsObject m_TargetNewsObject = null;
    private bool m_MoveToTarget = false;
    private Vector3 m_TargetPos;

    // ######################################### FUNCTIONS ########################################

    public void SetTargetPos(Vector3 _TargetPos)
    {
        m_MoveToTarget = true;
        m_TargetPos = _TargetPos;
    }

    public void LinkNews(WebManager.LinkData _LinkData)
    {
        if (m_LinkDataList.Count == 0) m_TargetNewsObject = _LinkData.linkNewsNodes.startNode;
        m_LinkDataList.Add(_LinkData);
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
        else {

            // If was firstNode, change target to endNode
            if (m_LinkDataList[0].linkNewsNodes.startNode == m_TargetNewsObject) {
                m_TargetNewsObject = m_LinkDataList[0].linkNewsNodes.endNode;
                m_LinkDataList[0].webLine.SetEndPoint(transform);
            }

            // Else change target to next linkData or set target to null
            else {
                m_LinkDataList[0].webLine.SetEndPoint(m_LinkDataList[0].linkNewsNodes.endNode.transform);
                m_LinkDataList.RemoveAt(0);
                m_TargetNewsObject = m_LinkDataList.Count > 0 ? m_LinkDataList[0].linkNewsNodes.startNode : null;
            }
        }
    }

    private void MoveToTargetPos()
    {
        // Move transform to target pos
        if (Vector3.Distance(m_TargetPos, transform.position) > m_DetectionRadius)
            UpdateMoveTo(m_TargetPos);

        // The spider reach the current target -> stop to move
        else m_MoveToTarget = false;
    }

    private void Update()
    {
        if (m_TargetNewsObject != null) MoveToTargetNewsObject();
        else if (m_MoveToTarget) MoveToTargetPos();
    }
}
