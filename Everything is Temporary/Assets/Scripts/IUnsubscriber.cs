using System;

/// <summary>
/// An "unsubscriber" in the observer pattern. Has a single Unsubscribe() method.
/// </summary>
public interface IUnsubscriber
{
    void Unsubscribe();
}

/// <summary>
/// An implementation of IUnsubscriber that uses a System.Action when
/// Unsubscribe() is invoked. Makes sure that multiple invocations of
/// Unsubscribe() do not use the action multiple times.
/// </summary>
public class ActionUnsubscriber : IUnsubscriber
{
    public ActionUnsubscriber(Action action)
    {
        m_action = action;
        m_used = false;
    }

    ~ActionUnsubscriber()
    {
        Unsubscribe();
    }

    public void Unsubscribe()
    {
        if (!m_used && m_action != null)
            m_action();

        m_used = true;
    }

    private Action m_action;
    private bool m_used;
}