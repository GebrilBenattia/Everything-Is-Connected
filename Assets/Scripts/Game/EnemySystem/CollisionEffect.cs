using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float m_LifeTime;
    void Start()
    {
        Destroy(gameObject, m_LifeTime);
    }
}
