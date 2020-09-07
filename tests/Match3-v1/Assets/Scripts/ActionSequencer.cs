using System.Collections.Generic;

public class ActionSequencer 
{
    private List<Action> m_actions = new List<Action>();

    public void Add(Action action)
    {
        m_actions.Add(action);
        if (m_actions.Count == 1)
        {
            action.OnStart();
        }
    }

    public void Update()
    {
        if (m_actions.Count > 0)
        {
            Action action = m_actions[0];
            action.Update();
            if (action.IsCompleted())
            {
                m_actions.RemoveAt(0);
                if (m_actions.Count > 0)
                {
                    m_actions[0].OnStart();
                }
            }
        }
    }

    public bool HasActions()
    {
        return m_actions.Count != 0;
    }
}
