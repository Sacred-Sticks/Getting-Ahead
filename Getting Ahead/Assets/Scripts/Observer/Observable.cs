using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observable : MonoBehaviour
{
    private readonly Dictionary<Type, object> observerLists = new Dictionary<Type, object>();

    public void AddObserver<T>(IObserver<T> observer)
    {
        if (!observerLists.ContainsKey(typeof(T)))
            observerLists.Add(typeof(T), new List<IObserver<T>>());
        var observers = GetObserverList<T>();
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver<T>(IObserver<T> observer)
    {
        if (!observerLists.ContainsKey(typeof(T)))
            observerLists.Add(typeof(T), new List<IObserver<T>>());
        var observers = GetObserverList<T>();
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    protected void NotifyObservers<T>(T argument)
    {
        if (!observerLists.ContainsKey(typeof(T)))
            return;
        var observers = GetObserverList<T>();
        for (int i = observers.Count - 1; i >= 0; i--)
            observers[i].OnNotify(argument);
    }

    private List<IObserver<T>> GetObserverList<T>() => (List<IObserver<T>>)observerLists[typeof(T)];
}