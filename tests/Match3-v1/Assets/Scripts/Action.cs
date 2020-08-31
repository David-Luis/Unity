public class Action
{
    private bool m_isCompleted = false;

    public bool IsCompleted()
    {
        return m_isCompleted;
    }

    public void Update()
    {

    }

    protected void Complete()
    {
        m_isCompleted = true;
    }
}
