using TMPro;
using UnityEngine;

public class GameForm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Healt;
    void Awake()
    {
        LockBrakeSystem.PicklockHealhChangeAction += (health) => { Healt.text = Mathf.Clamp(health,0,100).ToString();};
    }
    
}
