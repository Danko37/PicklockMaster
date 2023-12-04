using System;
using Cysharp.Threading.Tasks;

public class LockBrakeGameSystem : IGameSystem
{
    public class GameParams
    {
        public Lock Lock;
        public PickLock PickLock;
        public int Time;
    }

    public static event Action WinAction;
    public static event Action PickLockBrocked;
    public static event Action<bool> ErrorBreackAction;
    public static event Action<int> PicklockHealhChangeAction;

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

    public Lock CurrentLock { get; set; }

    public void InitGame(GameParams gameParams)
    {
        
    }

    public async UniTaskVoid Run()
    {
        PicklockRotator.PicklockRotationEvent += PicklockRotationEventHandler;
        CurrentLock.LockPhaseEvent += LockPhaseEventHandler;
        //LockRotator.LockRotationPressedEvent += b => _lockRotatePressed = b;

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

    private void LoadParams()
    {
        
    }

    public async UniTaskVoid UpdateSystem()
    {
        while (LockBrakeSystemIsRun)
        {
            /*var value = Mathf.Clamp(_curve.Evaluate(_picklockPhase),0f,1f);
        
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

            await UniTask.NextFrame();*/
        }
    }

    private void LockPhaseEventHandler(float phase) => _lockPhase = phase;

    private void PicklockRotationEventHandler(float phase) => _picklockPhase = phase;
    public string SystemName => nameof(LockBrakeGameSystem);
}
