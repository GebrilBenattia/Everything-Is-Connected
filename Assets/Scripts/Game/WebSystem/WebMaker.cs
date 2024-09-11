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
    [SerializeField] private float m_ClickCheckDelay;
    [SerializeField] private LayerMask m_NewsObjectLayer;
    private Camera m_Camera;
    private System.Action m_DoAction;
    private GameObject m_SelectedNode;
    private GameObject m_StartPoint;
    private GameObject m_EndPoint;
    private Vector3 m_CurrentStringStartPoint;
    private int m_SegmentsToSpawn;
    private int m_SegmentSpawned;
    private Vector3 m_LastNewsPos;
    private List<GameObject> m_CurrentNodes = new List<GameObject>();
    private float m_ElapsedTime;

    private List<GameObject> m_ActiveWebs = new List<GameObject>();
    private int m_WebsCreated;

    static public WebMaker instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Instance already exists, destroying the last one added");
            return;
        }
        instance = this;
    }

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
        if (CheckForNode() && Input.GetMouseButtonUp(0))
        {
            SetModeSowing();
            //if (Input.GetMouseButton(0) && !m_SelectedNode.GetComponent<NewsObject>().connected) 
            //{
            //    m_ElapsedTime += Time.deltaTime;
            //    if (m_ElapsedTime >= m_ClickCheckDelay)
            //    {
            //        m_ElapsedTime = 0f;
            //        SetModeNewsMove();
            //    }
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    SetModeSowing();
            //}
        }
    }

    private void SetModeSowing()
    {
        Debug.Log("STARTSOWING");
        m_StartPoint = m_SelectedNode;
        m_CurrentNodes.Add(m_StartPoint);
        m_DoAction = DoActionSowing;
    }

    private void DoActionSowing()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (CheckForNode() && m_SelectedNode != m_StartPoint)
            {
                m_EndPoint = m_SelectedNode;
                m_CurrentNodes.Add(m_EndPoint);
                m_CurrentStringStartPoint = m_StartPoint.transform.position;
                m_SegmentsToSpawn = (int)(Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position) / m_segmentLength);

                m_ActiveWebs.Insert(0, new GameObject());
                m_WebsCreated++;

                //AddSegment();

                for (int i = 0; i <= m_SegmentsToSpawn; i++)
                {
                    Quaternion rotation = Quaternion.LookRotation(m_EndPoint.transform.position - m_StartPoint.transform.position);
                    GameObject segment = Instantiate(m_StringSegmentToSpawn, m_CurrentStringStartPoint, rotation);
                    float distance = Vector3.Distance(m_StartPoint.transform.position, m_EndPoint.transform.position);
                    float ratio = (segment.GetComponentInChildren<WebSegment>().length / distance) * i;
                    Vector3 segmentPos = Vector3.Lerp(m_StartPoint.transform.position, m_EndPoint.transform.position, ratio);
                    segment.transform.position = segmentPos;
                    m_CurrentStringStartPoint = segment.GetComponentInChildren<WebSegment>().endPoint.position;
                    segment.transform.SetParent(m_ActiveWebs[0].transform, true);
                    segment.GetComponentInChildren<WebSegment>().webIndex = m_WebsCreated;
                    if (i == m_SegmentsToSpawn)
                    {
                        ClearValues();
                    }
                }

                foreach (GameObject news in m_CurrentNodes)
                {
                    news.GetComponent<NewsObject>().OnConneCtion();
                }
            }
            SetModeDefault();
        }
    }

    //private void SetModeNewsMove()
    //{
    //    m_LastNewsPos = m_SelectedNode.GetComponent<NewsObject>().initialPos;
    //    m_DoAction = DoActionNewsMove;
    //}

    //private void DoActionNewsMove()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        m_LastNewsPos = Vector3.zero;
    //        SetModeDefault();
    //        return;
    //    }
    //    Ray lRay = m_Camera.ScreenPointToRay(Input.mousePosition);
    //    Physics.Raycast(lRay, out RaycastHit hitInfo, m_RayDistance);        
    //    Vector3 mousePos = hitInfo.point;
    //    float ClampedX = Mathf.Clamp(mousePos.x, m_LastNewsPos.x - m_NewsMoveRange, m_LastNewsPos.x + m_NewsMoveRange);
    //    float ClampedZ = Mathf.Clamp(mousePos.z, m_LastNewsPos.z - m_NewsMoveRange, m_LastNewsPos.z + m_NewsMoveRange);
    //    Debug.Log(ClampedX);
    //    Debug.Log(ClampedZ);
    //    Vector3 velocity = new Vector3(ClampedX, m_SelectedNode.transform.position.y, ClampedZ);

    //    Debug.Log(velocity.x);
    //    m_SelectedNode.transform.position = velocity;
    //}

    //Check for NewsObject with raycast
    private bool CheckForNode()
    {
        bool lRaycast = false;

        Ray lRay = m_Camera.ScreenPointToRay(Input.mousePosition);

        bool lRayCastHit = Physics.Raycast(lRay, out RaycastHit hitInfo, m_RayDistance, m_NewsObjectLayer);

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
        float ratio = (segment.GetComponentInChildren<WebSegment>().length / distance) * m_SegmentSpawned;
        Vector3 segmentPos = Vector3.Lerp(m_StartPoint.transform.position, m_EndPoint.transform.position, ratio);
        segment.transform.position = segmentPos;
        m_CurrentStringStartPoint = segment.GetComponentInChildren<WebSegment>().endPoint.position;
        segment.transform.SetParent(m_ActiveWebs[0].transform, true);
        segment.GetComponentInChildren<WebSegment>().webIndex = m_WebsCreated;
        if (m_SegmentsToSpawn == 0)
        {
            ClearValues();
        }
        else Invoke(nameof(AddSegment), m_SegmentSpawnDelay);
    }

    private void ClearValues()
    {
        Debug.Log("cleared");
        m_CurrentNodes.Clear();
        m_SegmentSpawned = 0;
        m_SelectedNode = null;
        m_StartPoint = null;
        m_EndPoint = null;
    }

    public void OnWebSelected(int _Index)
    {
        int length = m_ActiveWebs.Count;
        for (int i = 0; i < length; i++)
        {
            if(m_ActiveWebs[i].transform.GetChild(i).GetComponentInChildren<WebSegment>().webIndex == _Index)
            {
                DeleteWeb(m_ActiveWebs[i]);
                break;
            }
        }

        if (m_ActiveWebs.Count == 0) m_WebsCreated = 0; 
    }

    private void DeleteWeb(GameObject _Web)
    {
        int length = _Web.transform.childCount;

        for (int i = 0; i < length; i++)
        {
            Destroy(_Web.transform.GetChild(i).gameObject);
        }

        DeleteContainer(_Web);
    }

    private void DeleteContainer(GameObject _Todelete)
    {
        Debug.Log("DESTROYED");
        m_ActiveWebs.Remove(_Todelete);
        Destroy(_Todelete);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}

