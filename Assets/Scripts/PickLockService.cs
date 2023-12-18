using Cysharp.Threading.Tasks;
using UnityEngine;

public class PickLockService : IGamesService
{
    private const string PICKLOCK_COUNT_PREF_KEY = "picklock_count";
    public int CurrentCount { get; set; }

    public PickLock CurrentPickLock { get; set; }

    public async UniTask Run()
    {
        CurrentCount = PlayerPrefs.GetInt(PICKLOCK_COUNT_PREF_KEY, 3);
    }

    public void SetCurrentPicklock(PickLock pickLock)
    {
        
    }

    public void SaveCurrentCount()
    {
        PlayerPrefs.SetInt(PICKLOCK_COUNT_PREF_KEY, CurrentCount);
        PlayerPrefs.Save();
    }

    public async UniTask UpdateSystem()
    { 
       
    }
    
    public string SystemName => nameof(PickLockService);
}
