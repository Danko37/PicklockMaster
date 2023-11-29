using System;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private GameObject WinForm;
    [SerializeField] private GameObject LoseForm;

    public static event Action RestartAction;
    private void Awake()
    {
        Cursor.visible = false;

        LockBrakeSystem.LoseAction += OnLoseActionHandler;
        LockBrakeSystem.WinAction += OnWinEventHandler;
    }

    private void OnLoseActionHandler()
    {
        LoseForm.SetActive(true);
        OnEndGame();
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
        RestartAction?.Invoke();
        
        WinForm.SetActive(false);
        LoseForm.SetActive(false);
        
        Cursor.visible = false;
    }
}
