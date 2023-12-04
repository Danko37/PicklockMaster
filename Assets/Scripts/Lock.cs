using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour
{
   public event Action<float> LockPhaseEvent;
   
   public AnimationCurve Curve;
   public List<Image> LockImages;

   public void LockRotationPhase(float phase)
   {
      
   }
}
