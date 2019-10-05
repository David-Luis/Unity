using TMPro;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int m_maxLife = 3;
    private int m_life;
    public TextMeshProUGUI m_text;

    public void Awake()
    {
        m_life = m_maxLife;
    }

    public void RemoveLife(int amount)
    {
        m_life -= amount;

        if (m_text != null)
        {
            m_text.text = "Lives: " + m_life + "/" + m_maxLife;
        }
    }

    public int GetLifes()
    {
        return m_life;
    }
}
