using System.Collections.Generic;

/// <summary>
/// Keeps a list of Unsubscribers alive. Provides an UnsubscribeAll() method
/// and calls it in the finalizer (in case this object is garbage collected).
/// </summary>
public class UnsubscriberList
{
    public UnsubscriberList()
    {
        mUnsubscribers = new List<IUnsubscriber>();
    }


    ~UnsubscriberList()
    {
        UnsubscribeAll();
    }


    public void Add(IUnsubscriber unsub)
    {
        mUnsubscribers.Add(unsub);
    }

    public void UnsubscribeAll()
    {
        foreach (var unsub in mUnsubscribers)
            unsub.Unsubscribe();

        mUnsubscribers.Clear();
    }

    private List<IUnsubscriber> mUnsubscribers;
}
