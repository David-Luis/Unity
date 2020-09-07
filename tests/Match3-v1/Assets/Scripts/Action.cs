public class Action
{
    private bool m_isCompleted = false;

    public bool IsCompleted()
    {
        return m_isCompleted;
    }

    public virtual void Update()
    {

    }

    public virtual void OnStart()
    {
    }

    protected virtual void Complete()
    {
        m_isCompleted = true;
    }
}
