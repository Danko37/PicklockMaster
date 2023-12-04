using Cysharp.Threading.Tasks;

public interface IGameSystem
{
    public string SystemName { get; }

    /// <summary>
    /// вызывается 1 раз при старте системы
    /// </summary>
    public async UniTask Run()
    {
    }
    
    //вызывается каждый кадр (реализвать)
    public async UniTask UpdateSystem()
    {
    }
    
}
