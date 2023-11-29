using System;
using Unity.VisualScripting;
using UnityEngine;

public class App : MonoBehaviour
{
    public static event Action RestartAction;
    
    [SerializeField] private GameObject WinForm;
    [SerializeField] private GameObject LoseForm;
    public static event Action<int> RoundFail;

    public int NumberОfTries { get; set; } = 3;

    private void Awake()
    {
        Cursor.visible = false;

        LockBrakeSystem.PickLockBrocked += OnPickLockBrockedHandler;
        LockBrakeSystem.WinAction += OnWinEventHandler;
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
