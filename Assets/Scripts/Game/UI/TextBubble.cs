using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBubble : MonoBehaviour
{
    [SerializeField] public GameObject[] Bubbles;
    public enum TextSizes
    {
        big,
        medium,
        small
    }

    public void Hide()
    {

    }

    public void SetNewBubble(TextSizes _Size, string _Text)
    {
        int length = transform.childCount;

        for (int i = 0; i < length; i++)
        {
            GameObject bubble = Bubbles[i];
            if (i == (int)_Size)
            {
                bubble.SetActive(true);
                bubble.GetComponentInChildren<Text>().text = _Text;
            }
            else bubble.SetActive(false);
        }
    }
}
