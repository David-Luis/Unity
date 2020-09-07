using UnityEngine;

public class Swappable : MonoBehaviour
{
    private static Swappable s_selected; 
    private SpriteRenderer m_renderer;
    private Tile m_tile;

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_tile = GetComponent<Tile>();
    }

    public void Select() 
    {
        m_renderer.color = Color.grey;
    }

    public void Unselect()  
    {
        m_renderer.color = Color.white;
    }

    private void OnMouseDown() 
    {
        if (!GridManager.Instance.CanInteract())
        {
            return;
        }

        if (s_selected != null)
        {
            if (s_selected == this)
                return;
            s_selected.Unselect();
            if (Vector2Int.Distance(s_selected.m_tile.Position, m_tile.Position) == 1)
            {
                GridManager.Instance.SwapTilesAndTryMatch(m_tile.Position, s_selected.m_tile.Position);
                s_selected = null;
            }
            else
            {
                s_selected = this;
                Select();
            }
        }
        else
        {
            s_selected = this;
            Select();
        }
    }
}
