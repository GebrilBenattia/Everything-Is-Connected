using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_LifeBar;
    private int testCounter = 8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) UpdateLife(testCounter--);
    }

    public void UpdateLife(int _Life)
    {
        m_LifeBar[_Life].GetComponent<Image>().enabled = false;
    }
}
