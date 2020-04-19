using Game.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class GameFinishedLabelController : MonoBehaviour
    {
        private bool _gameWon = false;
        private bool _gameFinished = false;
        private TextMeshProUGUI _text;
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            PlayerScore.Instance.OnGameLost += OnGameLost;
            PlayerScore.Instance.OnGameWon += OnGameWon;
        }

        private void OnGameWon()
        {
            _gameWon = true;
            _gameFinished = true;
            _text.enabled = true;
            _text.text = "GAME WON (Press R to reset, Press Escape to go back to main menu, Press Spacebar to continue playing)";
        }

        private void OnGameLost()
        {
            _gameFinished = true;
            _text.enabled = true;
            _text.text = "GAME LOST (Press R to reset, Press Escape to go back to main menu)";
        }

        private void Update()
        {
            if (_gameFinished)
            {
                if (Input.GetKeyUp(KeyCode.R))
                {
                    SceneManager.LoadScene("SampleScene");
                }
                else if (Input.GetKeyUp(KeyCode.Escape))
                {
                    SceneManager.LoadScene("MainMenu");
                }
                else if (Input.GetKeyUp(KeyCode.Space) && _gameWon)
                {
                    PlayerScore.Instance.ContinuePlaying();
                    _text.enabled = false;
                }
            }
        }

        private void OnDestroy()
        {
            PlayerScore.Instance.OnGameLost -= OnGameLost;
            PlayerScore.Instance.OnGameWon -= OnGameWon;
        }
    }
}