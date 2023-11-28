using System;
using UnityEngine;

public class LockRotator : MonoBehaviour
{
    public static event Action<float> LockRotationEvent;
    private RectTransform _lockRect;

    private float _phase = 0;
    private Vector3 _rotation;

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
                _phase += Time.deltaTime;
            }
        }
        else if(!Input.GetKey(KeyCode.D) && _phase >= 0f)
        {
            _phase -= Time.deltaTime;
        }

        LockRotationEvent?.Invoke(_phase);
        
        _rotation.z = Mathf.Lerp(0, -90, _phase);
        
        _lockRect.rotation = Quaternion.Euler(_rotation);
        //Debug.Log(isStopRotation);
        //Debug.Log($"Lock _phase = {_phase}, _rotation.z = _rotation.z = {_rotation.z},_lockRect.rotation = {_lockRect.rotation.eulerAngles} ");
    }
}
