using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Plants
{
    [RequireComponent(typeof(Plant), typeof(PlantWaterController))]
    public class PlantUiController : MonoBehaviour
    {
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _evolutionFinishedSound;

        private Plant _plant;
        private PlantWaterController _plantWater;
        [SerializeField]
        private GameObject _readyMarker;

        [SerializeField]
        private UIBar _healthBar;
        [SerializeField]
        private UIBar _waterBar;
        private void Start()
        {
            _plant = GetComponent<Plant>();
            _plantWater = GetComponent<PlantWaterController>();
            _audioSource = GetComponent<AudioSource>();

            _plant.OnPlantReadyToCrop += OnPlantReady;

            _healthBar.SetMax((int)_plant.MaxHealth);
            _plant.OnDamageTaken += OnHealthChanged;
            _plant.OnHealthAdded += OnHealthChanged;

            _waterBar.SetMax((int)_plantWater.MaxWater);
            Debug.Log(_plantWater.MaxWater);
            _plantWater.OnWaterChanged += OnWaterChanged;

            Plant.OnPlanted += OnPlanted;

            _healthBar.gameObject.SetActive(false);
            _waterBar.gameObject.SetActive(false);
        }

        private void OnPlanted(Plant plant)
        {
            if (_plant.IsPlanted)
            {
                _healthBar.gameObject.SetActive(true);
                _waterBar.gameObject.SetActive(true);
            }
        }

        private void OnWaterChanged(float amount, float currentWater)
        {
            _waterBar.SetValue((int)currentWater);
        }

        private void OnHealthChanged(float amount, float healthLeft)
        {
            _healthBar.SetValue((int)healthLeft);
        }

        private void OnPlantReady()
        {
            _audioSource.clip = _evolutionFinishedSound;
            _audioSource.Play();
            _readyMarker.SetActive(true);
        }

        private void OnDestroy()
        {
            _plant.OnPlantReadyToCrop -= OnPlantReady;
            _plant.OnDamageTaken -= OnHealthChanged;
            _plant.OnHealthAdded -= OnHealthChanged;
            _plantWater.OnWaterChanged -= OnWaterChanged;
            Plant.OnPlanted -= OnPlanted;
        }
    }
}