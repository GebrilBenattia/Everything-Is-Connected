using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBorderManager : MonoBehaviour
{
    // ######################################### SINGLETON ########################################

    private static GameBorderManager m_Instance;
    public static GameBorderManager instance
    { get { return m_Instance; } }

    // ######################################### VARIABLES ########################################

    // Object References
    [Header("Object References")]
    [SerializeField] private Camera m_Camera;

    // Colliders Settings
    [Header("Colliders Settings")]
    [SerializeField] private GameObject m_LeftCollider;
    [SerializeField] private GameObject m_RightCollider;
    [SerializeField] private GameObject m_UpCollider;
    [SerializeField] private GameObject m_DownCollider;

    // Trigger Settings
    [Header("Box Triggers Settings")]
    [SerializeField] private GameObject m_LeftTrigger;
    [SerializeField] private GameObject m_RightTrigger;
    [SerializeField] private GameObject m_UpTrigger;
    [SerializeField] private GameObject m_DownTrigger;

    // UI References
    [Header("UI Refernces")]
    [SerializeField] private RectTransform m_LeftDown;
    [SerializeField] private RectTransform m_RightDown;
    [SerializeField] private RectTransform m_LeftUp;
    [SerializeField] private RectTransform m_RightUp;

    [SerializeField] private Transform m_LeftDownSocket;
    [SerializeField] private Transform m_LeftUpSocket;
    [SerializeField] private Transform m_RightDownSocket;
    [SerializeField] private Transform m_RightUpSocket;

    // Private Variables
    private Vector3 m_WorldLeftDown;
    private Vector3 m_WorldRightDown;
    private Vector3 m_WorldLeftUp;
    private Vector3 m_WorldRightUp;
    private float m_LeftBorderSize;
    private float m_RightBorderSize;
    private float m_UpBorderSize;
    private float m_DownBorderSize;

    // ###################################### GETTER / SETTER #####################################

    public Vector3 worldLeftDown
    { get { return m_WorldLeftDown;} }

    public Vector3 worldRightDown
    {  get { return m_WorldRightDown;} }

    public Vector3 worldLeftUp
    {  get { return m_WorldLeftUp;} }

    public Vector3 worldRightUp
    { get { return m_WorldRightUp;} }

    public float leftBorderSize
    { get { return m_LeftBorderSize; } }

    public float rightBorderSize 
    { get {  return m_RightBorderSize; } }

    public float upBorderSize
    { get { return m_UpBorderSize; } }

    public float downBorderSize
    { get { return m_DownBorderSize; } }

    // ######################################### FUNCTIONS ########################################

    private void Awake()
    {
        m_Instance = this;
        //m_WorldLeftDown = m_Camera.ScreenToWorldPoint(m_LeftDown.position) + m_Camera.transform.position / 10f;
        //m_WorldRightDown = m_Camera.ScreenToWorldPoint(m_RightDown.position) + m_Camera.transform.position / 10f;
        //m_WorldLeftUp = m_Camera.ScreenToWorldPoint(m_LeftUp.position) + m_Camera.transform.position / 10f;
        //m_WorldRightUp = m_Camera.ScreenToWorldPoint(m_RightUp.position) + m_Camera.transform.position / 10f;

        m_WorldLeftDown = m_LeftDownSocket.position;
        m_WorldLeftUp = m_LeftUpSocket.position;
        m_WorldRightDown = m_RightDownSocket.position;
        m_WorldRightUp = m_RightUpSocket.position;

        m_WorldLeftDown.y = 0;
        m_WorldRightDown.y = 0;
        m_WorldLeftUp.y = 0;
        m_WorldRightUp.y = 0;

        // Calculate border sizes
        m_LeftBorderSize = Vector3.Distance(m_WorldLeftDown, m_WorldLeftUp);
        m_RightBorderSize = Vector3.Distance(m_WorldRightDown, m_WorldRightUp);
        m_UpBorderSize = Vector3.Distance(m_WorldLeftUp, m_WorldRightUp);
        m_DownBorderSize = Vector3.Distance(m_WorldLeftDown, m_WorldRightDown);

        // Set Box colliders
        m_LeftCollider.transform.position = (m_WorldLeftDown + m_WorldLeftUp) / 2f + new Vector3(-1, 0, 0);
        m_LeftCollider.transform.localScale = new Vector3(2, 2, m_LeftBorderSize);

        m_RightCollider.transform.position = (m_WorldRightDown + m_WorldRightUp) / 2f + new Vector3(1, 0, 0);
        m_RightCollider.transform.localScale = new Vector3(2, 2, m_RightBorderSize);

        m_UpCollider.transform.position = (m_WorldLeftUp + m_WorldRightUp) / 2f + new Vector3(0, 0, 1);
        m_UpCollider.transform.localScale = new Vector3(m_UpBorderSize, 2, 2);

        m_DownCollider.transform.position = (m_WorldLeftDown + m_WorldRightDown) / 2f + new Vector3(0, 0, -1);
        m_DownCollider.transform.localScale = new Vector3(m_DownBorderSize, 2, 2);

        // Set Box Triggers
        m_LeftTrigger.transform.position = (m_WorldLeftDown + m_WorldLeftUp) / 2f + new Vector3(-3, 0, 0);
        m_LeftTrigger.transform.localScale = new Vector3(2, 2, m_LeftBorderSize * 2f);

        m_RightTrigger.transform.position = (m_WorldRightDown + m_WorldRightUp) / 2f + new Vector3(3, 0, 0);
        m_RightTrigger.transform.localScale = new Vector3(2, 2, m_RightBorderSize * 2f);

        m_UpTrigger.transform.position = (m_WorldLeftUp + m_WorldRightUp) / 2f + new Vector3(0, 0, 3);
        m_UpTrigger.transform.localScale = new Vector3(m_UpBorderSize * 2f, 2, 2);

        m_DownTrigger.transform.position = (m_WorldLeftDown + m_WorldRightDown) / 2f + new Vector3(0, 0, -3);
        m_DownTrigger.transform.localScale = new Vector3(m_DownBorderSize * 2f, 2f, 2);
    }
}
