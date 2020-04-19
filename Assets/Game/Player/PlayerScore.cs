using Game.Plants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerScore : MonoBehaviour
    {
        public static PlayerScore Instance;
        public Spawner[] Spawners;

        [SerializeField]
        private float _finishedEvolutionScore = 500;
        [SerializeField]
        private float _evolutionScore = 10;
        [SerializeField]
        private float _plantScore = 5;

        [SerializeField]
        private float _gameWonScore = 2000;

        public float CurrentScore { get; private set; } = 0;
        public event Action<float, float> OnScoreChanged = delegate { };
        public event Action OnGameWon = delegate { };

        public event Action OnGameLost = delegate { };

        private int _plantsCount = 0;
        private void Awake()
        {
            Instance = this;
            Plant.OnPlanted += OnNewPlant;
            Plant.OnPlantDestroyed += OnPlantDestroyed;
        }

        private void OnDestroy()
        {
            Plant.OnPlanted -= OnNewPlant;
            Plant.OnPlantDestroyed -= OnPlantDestroyed;
        }

        private void OnPlantDestroyed()
        {
            _plantsCount--;
            Debug.Log($"Plant destroyed {_plantsCount}");
            if (_plantsCount <= 0)
            {
                Freeze();
                OnGameLost();
            }
        }

        private void OnNewPlant(Plant plant)
        {
            _plantsCount++;
            AddScore(_plantScore);

            plant.OnNextPlantStage += OnNextPlantStage;
            plant.OnPlantEvolutionFinished += OnPlantEvolutionFinished;
        }

        private void OnPlantEvolutionFinished()
        {
            AddScore(_finishedEvolutionScore);
            _plantsCount--;
        }

        private void OnNextPlantStage(int obj)
        {
            AddScore(_evolutionScore);
        }

        private void AddScore(float score)
        {
            CurrentScore += score;
            OnScoreChanged(CurrentScore, score);

            if (_gameWonScore <= CurrentScore)
            {
                Freeze();
                OnGameWon();
            }
        }

        private void Freeze()
        {
            foreach (var spanwer in Spawners)
            {
                spanwer.IsSpawning = false;
            }
            this.gameObject.SetActive(false);
        }

        public void ContinuePlaying()
        {
            _gameWonScore = 10000f;
            this.gameObject.SetActive(true);
            foreach (var spanwer in Spawners)
            {
                spanwer.IsSpawning = true;
            }
        }
    }
}