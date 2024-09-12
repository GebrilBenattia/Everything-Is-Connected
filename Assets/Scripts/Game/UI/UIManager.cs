using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_LifeBar;
    [SerializeField] private GameObject m_TextContainer;
    private int testCounter = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) UpdateContainers(m_TextContainer, testCounter--);
    }

    public void UpdateContainers(GameObject _Container, int _Life)
    {
         _Container.transform.GetChild(_Life).GetComponent<Image>().enabled = false;
    }
}
