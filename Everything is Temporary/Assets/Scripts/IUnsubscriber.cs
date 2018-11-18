using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
        if (!m_used)
        {
            Debug.LogWarning("ActionUnsubscriber died before Unsubscribe() was called." +
                             " Unsubscribe() will be called now.");
            Unsubscribe();
        }
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

/// <summary>
/// Removes an object from a list. Keeps a weak reference to both the list and
/// the object. Can optionally perform an action on Unsubscribe().
/// </summary>
public class ListUnsubscriber<T> : IUnsubscriber where T : class
{
    public ListUnsubscriber(List<T> lst, T obj, Action<T> action = null)
    {
        m_list = new WeakReference<List<T>>(lst);
        m_obj = new WeakReference<T>(obj);
        m_action = action;
        m_used = false;
    }

    ~ListUnsubscriber()
    {
        if (!m_used)
        {
            Debug.LogWarning("ListUnsubscriber died before Unsubscribe() was called." +
                             " Unsubscribe() will be called now.");
            Unsubscribe();
        }
    }

    public void Unsubscribe()
    {
        if (!m_used)
        {
            List<T> listRef;
            T objRef;


            if (!m_list.TryGetTarget(out listRef) || !m_obj.TryGetTarget(out objRef))
                m_used = true;
            else
            {
                listRef.Remove(objRef);
                m_action?.Invoke(objRef);
            }
        }

        m_used = true;
    }

    private bool m_used;
    private WeakReference<List<T>> m_list;
    private WeakReference<T> m_obj;
    private Action<T> m_action;
}