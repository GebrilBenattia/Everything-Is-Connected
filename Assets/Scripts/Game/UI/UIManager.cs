using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_LifeBar;
    [SerializeField] private GameObject m_TextContainer;
    private int testCounter = 0;

    static public UIManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("Instance already exists, destroying the last one added");
            return;
        }
        instance = this;
    }

    public void UpdateLife(GameObject _Container, int _Index)
    {
        _Container.transform.GetChild(_Index).GetComponent<Image>().enabled = false;
    }

    public void ResetBubbles()
    {
        Debug.Log("RESETTING");
        int length = m_TextContainer.transform.childCount;
        for (int i = 0; i < length; i++)
        {
           m_TextContainer.transform.GetChild(i).GetComponent<TextBubble>().HideAll();
        }
    }

    public void UpdateBubbles(int _Index, string _Text)
    {
        m_TextContainer.transform.GetChild(_Index).GetComponent<TextBubble>().SetNewBubble(TextBubble.TextSizes.small, _Text);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}
