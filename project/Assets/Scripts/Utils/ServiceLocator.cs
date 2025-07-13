using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class ServiceLocator
{
    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ServiceLocator();
            }
            return _instance;
        }
    }
    private static ServiceLocator _instance;
    private Dictionary<Type, object> _services;

    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void Reset()
    {
        _instance = null;
        _services.Clear();
    }

    public void RegisterService<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.Log($"Service {type} already registered");
        }
        _services.Add(type, service);
    }

    public T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            throw new Exception($"Service {type} not found");
        }
        return (T)service;
    }
}