using System;
using DG.Tweening;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
    private Transform _transform;
    public float speed;

    private Sequence RotateSequaence;

    private void Awake()
    {
        RotateSequaence?.Kill();
        RotateSequaence = null;
        
        RotateSequaence = DOTween.Sequence().SetUpdate(true).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart);
        RotateSequaence.Append(transform.DORotate(new Vector3(0, 0, 359), speed));
    }
}
