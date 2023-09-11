using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject 
{
    [SerializeField]
    List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(Component sender, object data)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
            listeners[i]?.OnEventRaised(sender, data);
    }

    public void RegisterListener(GameEventListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if(listeners.Contains(listener))
            listeners.Remove(listener);
    }
}