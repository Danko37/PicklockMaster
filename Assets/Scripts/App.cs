using System;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    public static event Action RestartAction;
    
    [SerializeField] private GameObject WinForm;
    [SerializeField] private GameObject LoseForm;
    public static event Action<int> RoundFail;

    public int NumberОfTries { get; set; } = 3;

    public List<IGameSystem> Systems = new()
    {
        new LockBrakeGameSystem()
    };

    private void Awake()
    {
        Cursor.visible = false;
        
        Systems.ForEach(system => {system.Run().Forget(); });

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
