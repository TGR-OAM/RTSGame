using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProccessBar : MonoBehaviour
{
   public Slider ProccessBarSlider;

   public Image ProcessBarBackground;

   public Gradient colorGradient;

   public void SetBarValue(float value)
   {
      ProccessBarSlider.value = Mathf.Clamp(value, 0 ,1f);
      UpdateSliderColorFromValue();

   }

   public void ChangeBarValue(float value)
   {
      ProccessBarSlider.value = Mathf.Clamp(ProccessBarSlider.value + value, 0 ,1f);
      UpdateSliderColorFromValue();
   }

   public float GetBarValue()
   {
      return ProccessBarSlider.value;
   }

   void UpdateSliderColorFromValue()
   {
      ProcessBarBackground.color = colorGradient.Evaluate(ProccessBarSlider.value);
   }
}
