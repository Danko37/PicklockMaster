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

    [SerializeField] private Transform FormsRoot;
    private static Transform _formsRoot;
    public int NumberОfTries { get; set; } = 3;

    /// <summary>
    /// Системы
    /// </summary>
    public List<IGamesService> Services = new()
    {
        new LockBrakeGamesService(),
        new PickLockService(),
        new FormsService(_formsRoot)
    };

    private async void Awake()
    {
        Cursor.visible = false;
        
        _formsRoot = FormsRoot;
        
        foreach (var system in Services)
        {
           await StaticSystemsProvider.Push(system);
        }

        LockBrakeGamesService.PickLockBrocked += OnPickLockBrockedHandler;
        LockBrakeGamesService.WinAction += OnWinEventHandler;

        StaticSystemsProvider.Get<FormsService>().ShowHideForm<MainGameForm>(true);
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

    private void OnApplicationQuit()
    {
        //при выходе из игры сохраняем сколько у нас есть отмычкек
        StaticSystemsProvider.Get<PickLockService>().SaveCurrentCount();
    }
}
