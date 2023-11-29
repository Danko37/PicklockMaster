using System;
using UnityEngine;

public class LockRotator : MonoBehaviour
{
    //событие поворота личинки замка (передает его фазу вращения)
    public static event Action<float> LockPhaseEvent;
    
    //событие нажатия на кнопку вращения личины замка
    public static event Action<bool> LockRotationPressedEvent;
    
    private RectTransform _lockRect;

    private float _phase = 0;
    private Vector3 _rotation;

    private bool _lockRotationPressed;
    
    //свойство для однократного вызова события нажатия на кнопку вращения личины
    public bool LockRotationPressed {
        get
        {
            return _lockRotationPressed;
        }
        private set
        {
            if (value != _lockRotationPressed)
            {
                LockRotationPressedEvent?.Invoke(value);  
            }

            _lockRotationPressed = value;
        }
    }

    private bool isStopRotation;
    private void Awake()
    {
        _lockRect = GetComponent<RectTransform>();
        _rotation = _lockRect.rotation.eulerAngles;

        LockBrakeSystem.ErrorBreackAction += ((value) => isStopRotation = value);
        App.RestartAction += (() => { _phase = 0f; });
    }

    void Update()
    {
        //_phase = Mathf.InverseLerp(0,-1,-Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.D) && _phase <= 1f)
        {
            if (!isStopRotation)
            {
                LockRotationPressed = true;
                _phase += Time.deltaTime;
            }
        }
        else if(!Input.GetKey(KeyCode.D) && _phase >= 0f)
        {
            LockRotationPressed = false;
            _phase -= Time.deltaTime;
        }

        LockPhaseEvent?.Invoke(_phase);
        
        _rotation.z = Mathf.Lerp(0, -90, _phase);
        
        _lockRect.rotation = Quaternion.Euler(_rotation);
        //Debug.Log(isStopRotation);
        //Debug.Log($"Lock _phase = {_phase}, _rotation.z = _rotation.z = {_rotation.z},_lockRect.rotation = {_lockRect.rotation.eulerAngles} ");
    }
}
