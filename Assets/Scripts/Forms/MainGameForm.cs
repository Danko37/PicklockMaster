using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainGameForm : UIFormBase
{
    [SerializeField] private List<GameObject> _picklockItems;

    private LockBrakeGamesService _lockBrakeGamesService;

    private void Start()
    {
        _lockBrakeGamesService =  StaticSystemsProvider.Get<LockBrakeGamesService>();
        
        App.RoundFail += SetPicklockItemsVisibility;
        App.RestartAction += () => 
        {
            foreach (var picklockItem in _picklockItems)
            {
                picklockItem.SetActive(true); 
            } 
        };
    }

    #region View
    private void  SetPicklockItemsVisibility(int triesLeft)
    {
        for (int i = 0; i < _picklockItems.Count - 1 ; i++)
        {
            _picklockItems[i].SetActive(i <= triesLeft - 1);
        }
    }

    #endregion

    #region Controller

    public void LoadLock(string lockPrefabName)
    {
        var currentLock = Resources.Load<Lock>($"Locks/{lockPrefabName}");
        var gameParams = new GameParams
        {
            Lock = currentLock
        };
        Instantiate(currentLock.gameObject, Vector3.zero, Quaternion.identity);
        _lockBrakeGamesService.InitGame(gameParams);
        _lockBrakeGamesService.UpdateSystem().Forget();
        StaticSystemsProvider.Get<FormsService>().ShowHideForm<MainGameForm>(false);
    }

    #endregion
}
