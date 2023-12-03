using System.Collections.Generic;
using UnityEngine;

public class MainGameForm : MonoBehaviour
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
        for (int i = 0; i < _picklockItems.Count - 1 ; i++)
        {
            _picklockItems[i].SetActive(i <= triesLeft - 1);
        }
    }

}
