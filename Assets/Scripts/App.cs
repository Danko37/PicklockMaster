using System;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    public static event Action RestartAction;
    public static event Action<int> RoundFail;
    
    [SerializeField] private GameObject WinForm;
    [SerializeField] private GameObject LoseForm;

    [SerializeField] private RectTransform LockRoot;
    
    public int NumberОfTries { get; set; } = 3;

    /// <summary>
    /// Системы
    /// </summary>
    public List<IGameSystem> Systems = new()
    {
        new LockBrakeGameSystem()
    };

    private async void Awake()
    {
        Cursor.visible = false;

        foreach (var system in Systems)
        {
           await StaticSystemsProvider.Push(system);
        }

        LockBrakeGameSystem.PickLockBrocked += OnPickLockBrockedHandler;
        LockBrakeGameSystem.WinAction += OnWinEventHandler;
    }

    private void OnPickLockBrockedHandler()
    {
        NumberОfTries -= 1;
        RoundFail?.Invoke(NumberОfTries);
        
        if (NumberОfTries <= 0)
        {
            LoseForm.SetActive(true);
            OnEndGame();
        }
    }

    private void OnWinEventHandler()
    {
        WinForm.SetActive(true);
        OnEndGame(); 
    }

    private void OnEndGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        NumberОfTries = 3;
        RestartAction?.Invoke();
        
        WinForm.SetActive(false);
        LoseForm.SetActive(false);
        
        Cursor.visible = false;
    }
}
