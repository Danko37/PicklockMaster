using Cysharp.Threading.Tasks;

public interface IGameSystem
{
    /// <summary>
    /// вызывается 1 раз при старте системы
    /// </summary>
    public async UniTaskVoid Run()
    {
    }
    
    //вызывается каждый кадр (реализвать)
    public async UniTaskVoid UpdateSystem()
    {
    }
    
}
