using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public static class StaticSystemsProvider
{
    private static List<IGamesService> _systems = new();

    public static async UniTask Push(IGamesService system)
    {
        if (!_systems.Contains(system))
        {
            await system.Run();
            _systems.Add(system);
        }
    }
    public static T Get<T>() where T : IGamesService
    {
        var result = _systems.Find(s => s.SystemName == typeof(T).Name);
        
        if (result != null)
        {
            return (T)result;
        }

        return default;
    }
}