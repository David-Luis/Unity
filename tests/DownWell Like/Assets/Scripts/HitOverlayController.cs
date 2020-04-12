using DG.Tweening;
using UnityEngine;

public class HitOverlayController : MonoBehaviour
{
    public float m_hitByEnemyTime = 0.15f;
    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HitByEnemy()
    {
        Color colorInit = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, 0.75f);

        m_spriteRenderer.DOColor(colorInit, 0.2f).SetLoops(2, LoopType.Yoyo).SetUpdate(true).OnComplete( ()=>
        {
            m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, 0.0f);
        });
    }


}
