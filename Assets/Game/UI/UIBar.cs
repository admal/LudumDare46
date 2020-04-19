using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Slider))]
    public class UIBar : MonoBehaviour
    {
        public Gradient gradient;

        private Slider _slider;
        [SerializeField]
        private Image _fill;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void SetMax(int value)
        {
            _slider.maxValue = value;
            _fill.color = gradient.Evaluate(1f);
        }

        public void SetValue(int value)
        {
            _slider.value = value;
            _fill.color = gradient.Evaluate(_slider.normalizedValue);
        }
    }
}