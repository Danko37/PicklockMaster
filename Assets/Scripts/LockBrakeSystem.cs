using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockBrakeSystem : MonoBehaviour
{
    public static event Action WinAction;
    public static event Action LoseAction;
    public static event Action<bool> ErrorBreackAction;
    public static event Action<int> PicklockHealhChangeAction;
    
    [SerializeField] private Image[] Lock;

    [SerializeField] private Color _normalLockColor = Color.white;
    [SerializeField] private Color _failLockColor = new Color(1,0.7f,0.7f);
    
    [SerializeField] private AnimationCurve _curve;


    private float _picklockHealh;
    
    public float PicklockHealh
    {
        get { return _picklockHealh;}
        set
        {
            _picklockHealh = value;
            PicklockHealhChangeAction?.Invoke((int)value);
        }} 

    private float _picklockPhase;
    private float _lockPhase;
    void Start()
    {
        PicklockHealh = 100;
        
        PicklockRotator.PicklockRotationEvent += PicklockRotationEventHandler;
        LockRotator.LockRotationEvent += LockRotationEventHandler;

        App.RestartAction += () =>
        {
            PicklockHealh = 100;
        };
    }

    // Update is called once per frame
    void Update()
    {
        var value = Mathf.Clamp(_curve.Evaluate(_picklockPhase),0f,1f);
        
        SetLockColor(value);

        if (_lockPhase > value)
        {
            ErrorBreackAction?.Invoke(true);
            PicklockHealh -= Time.deltaTime * 100;
        }
        else
        {
            ErrorBreackAction?.Invoke(false);
        }

        if (PicklockHealh <= 0f)
        {
            LoseAction?.Invoke();
        }

        if (_lockPhase >= 1f)
        {
            WinAction?.Invoke();
        }

        /*Debug.Log($"value {value}");
        Debug.Log($"_lockPhase {_lockPhase}");
        Debug.Log($"_picklockPhase {_picklockPhase}");*/
    }

    private void SetLockColor(float value)
    {
        if (_lockPhase > value)
        {
            foreach (var image in Lock)
            {
                image.color = _failLockColor;
            }
        }
        else
        {
            foreach (var image in Lock)
            {
                image.color = _normalLockColor;
            } 
        }
    }

    private void LockRotationEventHandler(float phase) => _lockPhase = phase;

    private void PicklockRotationEventHandler(float phase) => _picklockPhase = phase;
}
