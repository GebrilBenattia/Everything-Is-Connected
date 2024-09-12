using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_LifeBar;
    [SerializeField] private GameObject m_TextContainer;
    private int testCounter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1)) UpdateContainers(m_TextContainer, testCounter--);
        if (Input.GetMouseButtonDown(1)) UpdateBubbles(m_TextContainer, testCounter++);
    }

    public void UpdateLife(GameObject _Container, int _Index)
    {
        _Container.transform.GetChild(_Index).GetComponent<Image>().enabled = false;
    }

    public void UpdateBubbles(GameObject _Container, int _Index)
    {
        _Container.transform.GetChild(_Index).GetComponent<TextBubble>().SetNewBubble(TextBubble.TextSizes.small, "test");
    }

    //public void 
}
