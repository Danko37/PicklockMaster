using Cysharp.Threading.Tasks;

public interface IGamesService
{
    public string SystemName { get; }

    /// <summary>
    /// вызывается 1 раз при старте системы
    /// </summary>
    public UniTask Run();
    
    
    //вызывается каждый кадр (реализвать)
    public UniTask UpdateSystem();

}
