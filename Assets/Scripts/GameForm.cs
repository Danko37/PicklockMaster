using System.Collections.Generic;
using UnityEngine;

public class GameForm : MonoBehaviour
{
    [SerializeField] private List<GameObject> _picklockItems;

    void Awake()
    {
        App.RoundFail += SetPicklockItemsVisibility;
        App.RestartAction += () => 
        {
            foreach (var picklockItem in _picklockItems)
            {
                picklockItem.SetActive(true); 
            } };
    }

    private void  SetPicklockItemsVisibility(int triesLeft)
    {
        foreach (var picklockItem in _picklockItems)
        {
            if (picklockItem.activeSelf)
            {
                picklockItem.SetActive(false); 
            }
        }
    }

}
