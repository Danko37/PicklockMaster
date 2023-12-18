using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LockBrakeGamesService : IGamesService
{
    public static event Action WinAction;
    public static event Action PickLockBrocked;
    public static event Action<bool> ErrorBreackAction;
    public static event Action<int> PicklockHealhChangeAction;

    public GameParams CurrentGameParams;

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

    public void InitGame(GameParams gameParams)
    {
        CurrentGameParams = gameParams;
        CurrentGameParams.Lock.LockPhaseEvent += LockPhaseEventHandler;
    }

    public async UniTask Run()
    {
        PicklockRotator.PicklockRotationEvent += PicklockRotationEventHandler;
        
        //LockRotator.LockRotationPressedEvent += b => _lockRotatePressed = b;

        App.RestartAction += () =>
        {
            PicklockHealhProperty = 100;
            LockBrakeSystemIsRun = true;
        };
        
        LockBrakeSystemIsRun = true;

        await UniTask.Yield();
    }
    public async UniTask UpdateSystem()
    {
        while (LockBrakeSystemIsRun)
        {
            var value = Mathf.Clamp(CurrentGameParams.Lock.Curve.Evaluate(_picklockPhase),0f,1f);
        
            //SetLockColor(value);

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

    private void LockPhaseEventHandler(float phase) => _lockPhase = phase;

    private void PicklockRotationEventHandler(float phase) => _picklockPhase = phase;
    public string SystemName => nameof(LockBrakeGamesService);
}
