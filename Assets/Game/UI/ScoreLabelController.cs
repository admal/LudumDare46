using Game.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ScoreLabelController : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            PlayerScore.Instance.OnScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(float currentScore, float scoreChange)
        {
            _text.text = $"{currentScore}";
        }

        private void OnDestroy()
        {
            PlayerScore.Instance.OnScoreChanged -= OnScoreChanged;
        }
    }
}