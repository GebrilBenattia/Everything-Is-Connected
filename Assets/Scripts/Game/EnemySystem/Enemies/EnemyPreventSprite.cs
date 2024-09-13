using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyPreventSprite : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Object References
    [Header("Object References")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    // Private Variables
    private float m_BaseScale;

    // ######################################### FUNCTIONS ########################################

    private IEnumerator ScaleEffect(Vector3 _Start, Vector3 _End)
    {
        float t = 0;
        float speed = 2f;

        while (t < 1f) {
            t += Time.deltaTime * speed;
            m_SpriteRenderer.transform.localScale = Vector3.Lerp(_Start, _End, t);
            yield return null;
        }

        m_SpriteRenderer.transform.localScale = _End;
    }

    private void Awake()
    {
        m_BaseScale = transform.localScale.x;
    }

    public void Init(float _LifeTime)
    {
        StartCoroutine(ScaleEffect(Vector3.zero, Vector3.one * m_BaseScale));
        Invoke(nameof(Despawn), _LifeTime);
    }

    public void Despawn()
    {
        StartCoroutine(ScaleEffect(Vector3.one * m_BaseScale, Vector3.zero));
        Invoke(nameof(DestroySelf), 1f);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
