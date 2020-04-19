using Game.Items;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Plants
{
    [RequireComponent(typeof(EvolutionModifiers))]
    public class PlantWaterController : MonoBehaviour, IInteractable
    {
        public event Action OnDryPlant = delegate { };
        public event Action OnPlantWatered = delegate { };
        public event Action OnMaxWatered = delegate { };
        public event Action<float, float> OnWaterChanged = delegate { };

        private EvolutionModifiers _modifiers;
        
        private Plant _plant;

        private bool _recentlyFullyWatered = true;

        [SerializeField]
        private float _maxWaterStayTime = 15f;
        [SerializeField]
        private float _updateRatio = 5f;

        [SerializeField]
        private float _waterUseTick = 10f;
        [SerializeField]
        private float _maxWater = 100;
        public float MaxWater => _maxWater;
        private float _currentWater;

        private void Start()
        {
            _modifiers = GetComponent<EvolutionModifiers>();
            _plant = GetComponent<Plant>();
        }

        public void StartWaterSystem()
        {
            _currentWater = _maxWater;
            StartCoroutine(WaterUpdate());
        }

        public void ChangeWater(float waterAmount)
        {
            Debug.Log($"Change water: {waterAmount}");
            _currentWater = Mathf.Clamp(_currentWater + waterAmount, 0, _maxWater);
            OnWaterChanged(waterAmount, _currentWater);
            if (_currentWater == _maxWater)
            {
                _recentlyFullyWatered = true;
                OnMaxWatered();
            }
            UpdateModifiers();
        }

        private IEnumerator WaterUpdate()
        {
            while (true)
            {
                if (_recentlyFullyWatered)
                {
                    yield return new WaitForSeconds(_maxWaterStayTime);
                    _recentlyFullyWatered = false;
                }

                if (_currentWater > 0)
                {
                    ChangeWater(-_waterUseTick);
                }
                yield return new WaitForSeconds(_updateRatio);
            }
        }

        private void UpdateModifiers()
        {

            if (_currentWater == _maxWater)
            {
                _modifiers.WaterMultiplier = 1.2f;
            }
            else if (_currentWater == 0)
            {
                _modifiers.WaterMultiplier = 0;
            }
            else
            {
                _modifiers.WaterMultiplier = 1f;
            }
        }

        public bool Interact(GameObject item)
        {
            var wateringCan = item.GetComponent<WateringCan>();
            if (wateringCan != null && _plant.IsPlanted)
            {
                wateringCan.WaterPlant(this);
                OnPlantWatered();
                if (_currentWater == _maxWater)
                {
                    OnMaxWatered();
                }
            }

            return false;
        }

        public bool IsInteractable(GameObject item)
        {
            var wateringCan = item.GetComponent<WateringCan>();
            return wateringCan != null && wateringCan.CanUse && _plant.IsPlanted;
        }
    }
}