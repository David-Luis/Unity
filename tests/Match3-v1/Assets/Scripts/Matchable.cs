using UnityEngine;

public class Matchable : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool MatchWith(GameObject gameObject)
    {
        Matchable otherSpriteRenderer = gameObject.GetComponent<Matchable>();
        return m_spriteRenderer.sprite == otherSpriteRenderer.m_spriteRenderer.sprite;
    }
}
