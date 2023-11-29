using System;
using UnityEngine;
using UnityEngine.UI;

public class LockBrakeSystem : MonoBehaviour
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


    
    private bool _lockRotatePressed;
    private float _picklockPhase;
    private float _lockPhase;
    
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
    
    void Start()
    {
        PicklockHealhProperty = 100;
        
        PicklockRotator.PicklockRotationEvent += PicklockRotationEventHandler;
        LockRotator.LockPhaseEvent += LockPhaseEventHandler;

        App.RestartAction += () =>
        {
            PicklockHealhProperty = 100;
        };

        LockRotator.LockRotationPressedEvent += b => _lockRotatePressed = b;
    }

    // Update is called once per frame
    void Update()
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
            PickLockBrocked?.Invoke();
        }

        //замок открыт
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
