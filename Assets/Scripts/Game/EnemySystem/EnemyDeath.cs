using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyDeath : MonoBehaviour
{
    // ######################################### VARIABLES ########################################

    // Object References
    [Header("Object References")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    // Death Effects Settings
    [Header("Death Effects Settings")]
    [SerializeField] private float m_FadeSpeed;
    [SerializeField] private VisualEffect m_DeathEffect;
    [SerializeField] private Texture2D[] m_DeathEffectTextures;

    private void Awake()
    {
        StartCoroutine(DeathEffect());
    }

    private IEnumerator DeathEffect()
    {
        // Choose random texture for particles
        int textureIndex = Random.Range(0, m_DeathEffectTextures.Length);
        m_DeathEffect.SetTexture("mainTex", m_DeathEffectTextures[textureIndex]);

        // Play particles
        m_DeathEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        m_DeathEffect.gameObject.SetActive(false);

        float t = 1f;

        while (t > 0) {
            t -= Time.deltaTime * m_FadeSpeed;
            m_SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(t));
            yield return null;
        }

        Destroy(gameObject);
    }
}
