using System;
using System.Collections.Generic;

public static class EventBus
{
    // 타입 기반 이벤트 저장
    private static Dictionary<Type, Delegate> typedEventTable = new();

    // 문자열 기반 이벤트 저장
    public static Dictionary<string, Action<object>> stringEventTable = new();

    // void 이벤트 저장
    private static Dictionary<string, Action> voidEventTable = new();

    // 타입 + 문자열 기반 이벤트 저장
    private static Dictionary<(Type, string), Delegate> typedNamedEventTable = new();

    // 타입 기반 구독
    public static void Subscribe<T>(Action<T> callback)
    {
        if (typedEventTable.TryGetValue(typeof(T), out var del))
            typedEventTable[typeof(T)] = Delegate.Combine(del, callback);
        else
            typedEventTable[typeof(T)] = callback;
    }

    // 타입 기반 구독 해제
    public static void Unsubscribe<T>(Action<T> callback)
    {
        if (typedEventTable.TryGetValue(typeof(T), out var del))
        {
            var currentDel = Delegate.Remove(del, callback);
            if (currentDel == null)
                typedEventTable.Remove(typeof(T));
            else
                typedEventTable[typeof(T)] = currentDel;
        }
    }

    // 타입 기반 발행
    public static void Publish<T>(T evt)
    {
        if (typedEventTable.TryGetValue(typeof(T), out var del))
        {
            var callback = del as Action<T>;
            callback?.Invoke(evt);
        }
    }

    // 문자열 기반 구독
    public static void Subscribe(string eventName, Action<object> callback)
    {
        if (stringEventTable.TryGetValue(eventName, out var existing))
            stringEventTable[eventName] = existing + callback;
        else
            stringEventTable[eventName] = callback;
    }

    // 문자열 기반 구독 해제
    public static void Unsubscribe(string eventName, Action<object> callback)
    {
        if (stringEventTable.TryGetValue(eventName, out var existing))
        {
            existing -= callback;
            if (existing == null)
                stringEventTable.Remove(eventName);
            else
                stringEventTable[eventName] = existing;
        }
    }

    
    // 문자열 기반 발행
    public static void Publish(string eventName, object param = null)
    {
        if (stringEventTable.TryGetValue(eventName, out var callback))
        {
            callback?.Invoke(param);
        }
    }
    

    //모든 구독 내역 초기화
    public static void Clear()
    {
        typedEventTable = new();
        stringEventTable = new();
        voidEventTable = new();
        typedNamedEventTable = new();
    }
    
    public static void SubscribeVoid(string eventName, Action callback)
    {
        if (voidEventTable.TryGetValue(eventName, out var existing))
            voidEventTable[eventName] = existing + callback;
        else
            voidEventTable[eventName] = callback;
    }

    public static void UnsubscribeVoid(string eventName, Action callback)
    {
        if (voidEventTable.TryGetValue(eventName, out var existing))
        {
            existing -= callback;
            if (existing == null)
                voidEventTable.Remove(eventName);
            else
                voidEventTable[eventName] = existing;
        }
    }

    public static void PublishVoid(string eventName)
    {
        if (voidEventTable.TryGetValue(eventName, out var callback))
        {
            callback?.Invoke();
        }
    }

    public static void ClearEvent(string eventName)
    {
        stringEventTable.Remove(eventName);
        voidEventTable.Remove(eventName);
    }

    public static void Subscribe<T>(string eventName, Action<T> callback)
    {
        var key = (typeof(T), eventName);
        if (typedNamedEventTable.TryGetValue(key, out var del))
            typedNamedEventTable[key] = Delegate.Combine(del, callback);
        else
            typedNamedEventTable[key] = callback;
    }

    public static void Unsubscribe<T>(string eventName, Action<T> callback)
    {
        var key = (typeof(T), eventName);
        if (typedNamedEventTable.TryGetValue(key, out var del))
        {
            var currentDel = Delegate.Remove(del, callback);
            if (currentDel == null)
                typedNamedEventTable.Remove(key);
            else
                typedNamedEventTable[key] = currentDel;
        }
    }

    public static void Publish<T>(string eventName, T evt)
    {
        var key = (typeof(T), eventName);
        if (typedNamedEventTable.TryGetValue(key, out var del))
        {
            var callback = del as Action<T>;
            callback?.Invoke(evt);
        }
    }

    public static void Clear(Type type, string eventName)
    {
        var key = (type, eventName);
        if (typedNamedEventTable.ContainsKey(key))
        {
            typedNamedEventTable.Remove(key);
        }
    }

    public static void Clear<T>(string eventName)
    {
        Clear(typeof(T), eventName);
    }
    
}