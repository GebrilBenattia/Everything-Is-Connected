using FMOD.Studio;
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
    [SerializeField] private int m_MaxWebLineSelection;
    [SerializeField] private int m_MaxWebLines;

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

    public void DeselectNewsNodes()
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
        if (m_SpiderController.currentLinkCount >= m_MaxWebLineSelection) return;
        if (m_LinkDataList.Count >= m_MaxWebLines) return;

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
        /*Debug.Log(m_CurrentLinkNewsNodes.startNode.newsData.name);
        Debug.Log(m_CurrentLinkNewsNodes.endNode.newsData.name);*/
        float damage = GameplayManager.Instance.newsLinkData.Get(m_CurrentLinkNewsNodes.startNode.newsData.guid, m_CurrentLinkNewsNodes.endNode.newsData.guid);

        // Instantiate new web line
        WebLine webLineInst = Instantiate(m_WebPrefab, Vector3.zero, Quaternion.identity, m_WebsParent).GetComponent<WebLine>();
        webLineInst.Init(m_CurrentLinkNewsNodes.startNode.transform, m_CurrentLinkNewsNodes.startNode.transform, damage);

        // Add new Link Data
        LinkData newLinkData = new LinkData {
            linkNewsNodes = m_CurrentLinkNewsNodes,
            webLine = webLineInst
        };
        m_LinkDataList.Add(newLinkData);
        
        // Start link news with spider
        m_SpiderController.LinkNews(newLinkData);
    }

    public void CompletelyUnlink(NewsObject _NewsObject)
    {
        // If Current link news satrt node is _NewsObject -> Deselect news nodes
        if (m_CurrentLinkNewsNodes.startNode == _NewsObject) DeselectNewsNodes();

        // Remove all link from the newsObject in spider
        spiderController.RemoveAllLinkFromNewsObject(_NewsObject);

        // Loop on each link data
        for (int i = 0; i <  m_LinkDataList.Count;) {

            // Check if the News Object is in the current link Data
            if (m_LinkDataList[i].linkNewsNodes.startNode == _NewsObject ||
                m_LinkDataList[i].linkNewsNodes.endNode == _NewsObject) {

                // Unlink all news objects
                m_LinkDataList[i].linkNewsNodes.startNode.EventOnUnlink(m_LinkDataList[i].linkNewsNodes.endNode);
                m_LinkDataList[i].linkNewsNodes.endNode.EventOnUnlink(m_LinkDataList[i].linkNewsNodes.startNode);
                
                // Destroy web line
                m_LinkDataList[i].webLine.DestroyLine();

                // Remove current link data
                m_LinkDataList.RemoveAt(i);
            }
            else ++i;
        }
    }

    public void BreakWebLine(WebLine _WebLine)
    {
        // Loop on each link data
        for (int i = 0; i < m_LinkDataList.Count; ++i) {

            // Check if the News Object is in the current link Data
            if (m_LinkDataList[i].webLine == _WebLine) {

                // Unlink all news objects
                m_LinkDataList[i].linkNewsNodes.startNode.EventOnUnlink(m_LinkDataList[i].linkNewsNodes.endNode);
                m_LinkDataList[i].linkNewsNodes.endNode.EventOnUnlink(m_LinkDataList[i].linkNewsNodes.startNode);

                // Destroy web line
                m_LinkDataList[i].webLine.DestroyLine();

                // Remove current link data
                m_LinkDataList.RemoveAt(i);

                if (m_SpiderController.currentLinkData.webLine == _WebLine)
                    m_SpiderController.StopCurrentLink();

                break;
            }
        }
    }
}
