using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PicklockRotator : MonoBehaviour
{
    public static event Action<float> PicklockRotationEvent;
    private RectTransform _pickLockRect;
    private bool _locked;

    private float _phase = 0.5f;
    private Vector3 _rotation;
    private void Awake()
    {
        _pickLockRect = GetComponent<RectTransform>();
        _rotation = _pickLockRect.rotation.eulerAngles;
        App.RestartAction += (() => { _phase = 0.5f; });
        LockRotator.LockRotationPressedEvent += b => _locked = b;
    }

    void Update()
    {
        if (!_locked)
        {
            _phase += Input.GetAxis("Mouse X") * Time.deltaTime * 10f;
        }
        
        _phase = Math.Clamp(_phase, 0f, 1f);
        
        PicklockRotationEvent?.Invoke(_phase);

        _rotation.z = Mathf.Lerp(90, -90, _phase);
        
        _pickLockRect.rotation = Quaternion.Euler(_rotation);
        
        //Debug.Log($"Picklock _phase = {_phase}, _rotation.z = _rotation.z = {_rotation.z},_lockRect.rotation = {_lockRect.rotation.eulerAngles} ");
    }
}
