using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadLine : MonoBehaviour
{
    [SerializeField] private Image m_InactiveHeadline;
    [SerializeField] private Image m_ActiveHeadline;
    [SerializeField] private GameObject m_ScrollingText;
    [SerializeField] private float m_ScrollSpeed;

    [SerializeField] private float _StartPosX;
    [SerializeField] private float _EndPosX;
    private System.Action doAction;

    private void SetModeVoid()
    {
        m_ActiveHeadline.enabled = false;
        m_InactiveHeadline.enabled = true;
        doAction = DoActionVoid;
    }

    private void DoActionVoid()
    {
        
    }

    public void SetModeScroll(string _Text)
    {
        m_InactiveHeadline.enabled = false;
        m_ActiveHeadline.enabled = true;
        m_ScrollingText.GetComponent<Text>().text = _Text;
        doAction = DoActionScroll;
    }

    private void DoActionScroll()
    {
        if(Input.GetKeyDown(KeyCode.D))Debug.Log(m_ScrollingText.GetComponent<RectTransform>().position.x);
        if(m_ScrollingText.GetComponent<RectTransform>().position.x < _EndPosX)
        {
            m_ScrollingText.transform.position = new Vector3(_StartPosX, m_ScrollingText.transform.position.y, m_ScrollingText.transform.position.z);
            SetModeVoid();
        }
        m_ScrollingText.transform.position += Vector3.left * 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetModeVoid();
    }

    // Update is called once per frame
    void Update()
    {
        doAction();
    }
}
