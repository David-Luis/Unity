using System.Collections;
using System.Collections.Generic;

public class ActionSequencer 
{
    private List<Action> m_actions;

    public void Add(Action action)
    {
        m_actions.Add(action);
    }

    public void Update()
    {
        foreach (Action action in m_actions)
        {
            action.Update();
        }

        m_actions.RemoveAll(IsCompleted);
    }

    private static bool IsCompleted(Action action)
    {
        return action.IsCompleted();
    }
}
