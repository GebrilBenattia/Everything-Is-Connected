using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    // ########################################## STRUCTS #########################################

    [System.Serializable]
    public struct LinkNewsNodes
    {
        public NewsObject startNode;
        public NewsObject endNode;
    }

    [System.Serializable]
    public struct LinkData
    {
        public LinkNewsNodes linkNewsNodes;
        public WebLine webLine;
    }

    // ######################################### SINGLETON ########################################

    private static WebManager m_Instance;
    public static WebManager instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

    // Object References
    [Header("Object References")]
    [SerializeField] private SpiderController m_SpiderController;

    // Web Settings
    [Header("Web Settings")]
    [SerializeField] private Transform m_WebsParent;
    [SerializeField] private GameObject m_WebPrefab;

    // Private Variables
    private List<LinkData> m_LinkDataList = new List<LinkData>();
    private LinkNewsNodes m_CurrentLinkNewsNodes;

    // ###################################### GETTER / SETTER #####################################

    public SpiderController spiderController
    { get { return m_SpiderController; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
    }

    public void DeselectNewsNode()
    {
        m_CurrentLinkNewsNodes = new LinkNewsNodes();
    }

    public bool AreAlreadyLinked(LinkNewsNodes _LinkNewsNodes)
    {
        // Loop on each link data
        for (int i = 0; i < m_LinkDataList.Count; ++i) {

            // Check if linkNewsNodes are already linked
            LinkNewsNodes nodes = m_LinkDataList[i].linkNewsNodes;
            if (nodes.startNode == _LinkNewsNodes.startNode && nodes.endNode == _LinkNewsNodes.endNode || 
                nodes.startNode == _LinkNewsNodes.endNode && nodes.endNode == _LinkNewsNodes.startNode)
                return true;
        }

        return false;
    }

    public bool AreSameNodes(LinkNewsNodes _LinkNewsNodes)
    {
        return _LinkNewsNodes.startNode == _LinkNewsNodes.endNode;
    }

    public void AddNewsObjectAsLinkNode(NewsObject _NewsObject)
    {
        // Set spider targetPos
        m_SpiderController.SetTargetPos(_NewsObject.transform.position);

        // Set start Node
        if (m_CurrentLinkNewsNodes.startNode == null) m_CurrentLinkNewsNodes.startNode = _NewsObject;

        // Set end Node
        else {

            // Check if can create the link and add to nodes list
            m_CurrentLinkNewsNodes.endNode = _NewsObject;
            if (!AreAlreadyLinked(m_CurrentLinkNewsNodes) && !AreSameNodes(m_CurrentLinkNewsNodes)) AddNewLinkData();
            m_CurrentLinkNewsNodes = new LinkNewsNodes();
        }
    }

    private void AddNewLinkData()
    {
        // Instantiate new web line
        WebLine webLineInst = Instantiate(m_WebPrefab, Vector3.zero, Quaternion.identity, m_WebsParent).GetComponent<WebLine>();
        webLineInst.Init(m_CurrentLinkNewsNodes.startNode.transform, m_CurrentLinkNewsNodes.startNode.transform, 1);

        // Add new Link Data
        LinkData newLinkData = new LinkData {
            linkNewsNodes = m_CurrentLinkNewsNodes,
            webLine = webLineInst
        };
        m_LinkDataList.Add(newLinkData);

        // Start link news with spider
        m_SpiderController.LinkNews(newLinkData);
    }
}
