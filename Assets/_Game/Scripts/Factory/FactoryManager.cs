using System;
using System.Collections.Generic;

public class FactoryManager
{
    private readonly Dictionary<Type, object> _factories = new();

    public void RegisterFactory<T>(BaseFactory<T> factory) where T : UnityEngine.Object
    {
        var type = typeof(T);

        if (_factories.ContainsKey(type))
            throw new ArgumentException($"Factory for type '{type}' is already registered.");

        _factories[type] = factory;
    }
    public T Create<T>(params object[] args) where T : UnityEngine.Object
    {
        var type = typeof(T);

        if (!_factories.ContainsKey(type))
            throw new KeyNotFoundException($"Factory for type '{type}' is not registered.");

        if (_factories[type] is not BaseFactory<T> factory)
            throw new InvalidCastException($"Factory for type '{type}' is not compatible with type {typeof(T)}.");

        return factory.Create(args);
    }
    public FactoryManager InitializeFactories()
    {
        _factories.Clear();
        RegisterFactory(new BuildingFactory());
        RegisterFactory(new BuildingPreviewFactory());
        RegisterFactory(new UnitFactory());
        return this;
    }
}
