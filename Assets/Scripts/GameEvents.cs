using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    private Dictionary<string, UnityEvent<int>> eventDictionary;
    private static GameEvents current;
    public static GameEvents instance
    {
        get
        {
            if (!current)
            {
                current = FindObjectOfType(typeof(GameEvents)) as GameEvents;
                if (!current)
                {
                    Debug.LogError("There needs to be one active GameEvent script on a GameObject in your scene.");
                }
                else
                {
                    current.Init();
                }
            }
            return current;
        }
    }
 
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent<int>>();
        }
    }

    public static void StartListening(string eventName, UnityAction<int> listener)
    {
        UnityEvent<int> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<int>();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<int> listener)
    {
        if (current == null) return;
 
        UnityEvent<int> thisEvent;
 
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, int value)
    {
        UnityEvent<int> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
}
