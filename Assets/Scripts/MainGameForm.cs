using System;
using System.Collections.Generic;
using UnityEngine;

public class MainGameForm : MonoBehaviour
{
    [SerializeField] private List<GameObject> _picklockItems;

    [SerializeField] private GameObject SelectLockPanel;

    private LockBrakeGameSystem _lockBrakeGameSystem;

    private void Start()
    {
        _lockBrakeGameSystem =  StaticSystemsProvider.Get<LockBrakeGameSystem>();
        
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
    }

    #endregion
}
