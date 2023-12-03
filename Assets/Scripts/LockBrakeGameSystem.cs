using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LockBrakeGameSystem : IGameSystem
{
    public static event Action WinAction;
    public static event Action PickLockBrocked;
    public static event Action<bool> ErrorBreackAction;
    public static event Action<int> PicklockHealhChangeAction;
    
    [SerializeField] private Image[] Lock;

    [SerializeField] private Color _normalLockColor = Color.white;
    [SerializeField] private Color _failLockColor = new Color(1,0.7f,0.7f);
    
    /// <summary>
    /// Описание сложности замка 
    /// </summary>
    [SerializeField] private AnimationCurve _curve;
    
    //врощаем личину
    private bool _lockRotatePressed;
    
    // на сколько повернута отмыска
    private float _picklockPhase;
    
    // на сколько повернута личина
    private float _lockPhase;
    
    // здоровье отмычки
    private float _picklockHealh;
    public float PicklockHealhProperty
    {
        get { return _picklockHealh;}
        set
        {
            _picklockHealh = value;
            PicklockHealhChangeAction?.Invoke((int)value);
        }
    }


    private bool _errorBreack;
    public bool ErrorBreackProperty
    {
        get { return _errorBreack; }

        private set
        {
            if (value != _errorBreack)
            {
                ErrorBreackAction?.Invoke(value);
                _errorBreack = value;
            }
        }
    }

    private static bool _lockBrakeSystemIsRun;
    public static bool LockBrakeSystemIsRun
    {
        get { return _lockBrakeSystemIsRun; }

        set
        {
            _lockBrakeSystemIsRun = value;
        }
    }

    public async UniTaskVoid Run()
    {
        PicklockHealhProperty = 100;
        
        PicklockRotator.PicklockRotationEvent += PicklockRotationEventHandler;
        LockRotator.LockPhaseEvent += LockPhaseEventHandler;
        LockRotator.LockRotationPressedEvent += b => _lockRotatePressed = b;

        App.RestartAction += () =>
        {
            PicklockHealhProperty = 100;
            LockBrakeSystemIsRun = true;
            UpdateSystem().Forget();
        };
        
        LockBrakeSystemIsRun = true;
        
        UpdateSystem().Forget();
        
        await UniTask.Yield();
    }

    public async UniTaskVoid UpdateSystem()
    {
        while (LockBrakeSystemIsRun)
        {
            var value = Mathf.Clamp(_curve.Evaluate(_picklockPhase),0f,1f);
        
            SetLockColor(value);

            //событие не правильного отпирания
            if (_lockPhase > value && _lockRotatePressed)
            {
                ErrorBreackProperty = true;
                PicklockHealhProperty -= Time.deltaTime * 100;
            }
            else
            {
                ErrorBreackProperty = false;
            }

            //отмычка сломалась
            if (PicklockHealhProperty <= 0f)
            {
                LockBrakeSystemIsRun = false;
                PickLockBrocked?.Invoke();
            }

            //замок открыт
            if (_lockPhase >= 1f)
            {
                WinAction?.Invoke();
            }

            await UniTask.NextFrame();
        }
    }

    private void SetLockColor(float value)
    {
        if (_lockPhase > value && _lockRotatePressed)
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

    private void LockPhaseEventHandler(float phase) => _lockPhase = phase;

    private void PicklockRotationEventHandler(float phase) => _picklockPhase = phase;
    
}
