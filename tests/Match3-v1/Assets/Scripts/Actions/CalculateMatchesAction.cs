using UnityEngine;
using System.Collections.Generic;

public class CalculateMatchesAction : Action
{
    private Vector2Int m_tile1Position;
    private Vector2Int m_tile2Position;
    private bool m_fromSwap;

    public CalculateMatchesAction(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        m_tile1Position = tile1Position;
        m_tile2Position = tile2Position;
        m_fromSwap = true;
    }

    public CalculateMatchesAction()
    {
        m_fromSwap = false;
    }

    public override void OnStart()
    {
        bool hasMatches = GridManager.Instance.CalculateMatches();
        if (hasMatches)
        {
            GridManager.Instance.ActionSequencer.Add(new MatchAnimationAction(GridManager.Instance.MatchedBoardObjects));
            GridManager.Instance.ActionSequencer.Add(new ResolveMatchesAction());
        }
        else if (m_fromSwap)
        {
            GridManager.Instance.ActionSequencer.Add(new SwapAction(m_tile1Position, m_tile2Position, tryMatch: false));
        }

        Complete();
    }
}

