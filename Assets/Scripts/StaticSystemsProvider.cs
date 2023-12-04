using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public static class StaticSystemsProvider
{
    private static List<IGameSystem> _systems = new();

    public static async UniTask Push(IGameSystem system)
    {
        if (!_systems.Contains(system))
        {
            await system.Run();
            _systems.Add(system);
        }
    }
    public static T Get<T>() where T : IGameSystem
    {
        var result = _systems.Find(s => s.SystemName == typeof(T).Name);
        
        if (result != null)
        {
            return (T)result;
        }

        return default;
    }
}