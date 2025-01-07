using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    /// <summary>
    /// Registers a service instance.
    /// </summary>
    public static void Register<T>(T service)
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
        {
            Debug.LogWarning($"Service of type {type} is already registered.");
            return;
        }

        services[type] = service;
     
    }

    /// <summary>
    /// Retrieves a service instance.
    /// </summary>
    public static T Get<T>()
    {
        var type = typeof(T);

        if (services.TryGetValue(type, out var service))
        {
            return (T)service;
        }
        Debug.LogError("Service cannot found");
        throw new Exception($"Service of type {type} is not registered.");
    }

    /// <summary>
    /// Clears all registered services (e.g., when resetting the game state).
    /// </summary>
    public static void Clear()
    {
        services.Clear();
    }
}
