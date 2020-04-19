using Game.Damage;
using Game.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Plants
{
    [RequireComponent(typeof(EvolutionModifiers), typeof(PlantWaterController))]
    public class Plant : MonoBehaviour, IDamagable, IInteractable
    {
        public GameObject EnemyArrow;

        private AudioSource _audioSource;
        [SerializeField]
        public AudioClip _hammerHoeUsageSound;
        [SerializeField]
        private AudioClip _plantAttackedSound;
        [SerializeField]
        private AudioClip _plantDestroyed;
        
        [SerializeField]
        private GameObject _plantAttackedParticles;
        [SerializeField]
        private GameObject _plantDestroyedParticles;
        [SerializeField]
        private GameObject _mesh;


        public bool PlantIsFinished = false;
        public bool IsPlanted = false;
        private EvolutionModifiers _modifiers;
        private AvailablePlant _plantPlace;

        [SerializeField]
        private float _evolutionRate = 1f;

        private float _evolutionValue;
        private PlantEvolutionStage[] _stages;
        private int _currentStage = 0;

        public event Action<int> OnNextPlantStage = delegate { };
        public event Action OnPlantEvolutionFinished = delegate { };
        public event Action OnPlantReadyToCrop = delegate { };

        public static event Action<Plant> OnPlanted = delegate { };
        public static event Action OnPlantDestroyed = delegate { };

        //TODO: make it into proper health component
        [SerializeField]
        private float _maxHealth = 100;
        public float MaxHealth => _maxHealth;

        private float _healthPoints;

        /// <summary>
        /// arg1 - damage value
        /// arg2 - health left
        /// </summary>
        public event Action<float, float> OnDamageTaken = delegate { };
        public event Action<float, float> OnHealthAdded = delegate { };

        private IEnumerator UpdateEvolutionRate()
        {
            while (true)
            {
                yield return new WaitForSeconds(_evolutionRate);
                _modifiers.HealthMultipier = _healthPoints / _maxHealth;
                _evolutionValue += _modifiers.GetEvolutionChange();

                if (_evolutionValue > _stages[_currentStage].StageLimit)
                {

                    _currentStage++;

                    if (_currentStage >= _stages.Length)
                    {
                        break;
                    }

                    _stages[_currentStage - 1].gameObject.SetActive(false);
                    _stages[_currentStage].gameObject.SetActive(true);
                    _evolutionValue = 0;

                    OnNextPlantStage(_currentStage + 1);
                }
            }

            PlantIsFinished = true;
            OnPlantReadyToCrop();
        }

        public void AddHealth(float health)
        {
            _healthPoints = Mathf.Clamp(_healthPoints + health, 0, _maxHealth);
            OnHealthAdded(health, _healthPoints);
        }

        public bool ApplyDamage(DamageInfo damage)
        {
            _healthPoints -= damage.BaseDamage;
            OnDamageTaken(damage.BaseDamage, _healthPoints);

            if (_healthPoints <= 0)
            {
                _audioSource.clip = _plantDestroyed;
                _plantDestroyedParticles.SetActive(true);
                OnPlantDestroyed();
                _mesh.SetActive(false);
                _stages[_currentStage].gameObject.SetActive(false);
                Destroy(this.gameObject, 0.1f);
                _plantPlace.gameObject.SetActive(true);
            }
            else
            {
                _plantAttackedParticles.SetActive(true);
                _audioSource.clip = _plantAttackedSound;
            }
            _audioSource.Play();
            return true;
        }

        /// <summary>
        /// Best naming of all time
        /// </summary>
        public void PlantThePlant(AvailablePlant plantPlace)
        {
            IsPlanted = true;
            _healthPoints = _maxHealth;
            _modifiers = GetComponent<EvolutionModifiers>();
            _audioSource = GetComponent<AudioSource>();
            _stages = GetComponentsInChildren<PlantEvolutionStage>(true);

            GetComponent<DummyItem>().IsPickable = false;
            GetComponent<PlantWaterController>().StartWaterSystem();
            _plantPlace = plantPlace;
            OnPlanted(this);
            StartCoroutine(UpdateEvolutionRate());
        }

        public void CropPlant()
        {
            if (PlantIsFinished)
            {
                _audioSource.clip = _hammerHoeUsageSound;
                _audioSource.Play();
                OnPlantEvolutionFinished();
                Destroy(gameObject, 0.1f);
                _plantPlace.gameObject.SetActive(true);
            }
        }

        public bool Interact(GameObject item)
        {
            var hoe = item.GetComponent<Hoe>();
            if (hoe != null && PlantIsFinished)
            {
                CropPlant();
            }

            var hammer = item.GetComponent<Hammer>();
            if (hammer != null)
            {
                hammer.RepairPlant(this);
                _audioSource.clip = _hammerHoeUsageSound;
                _audioSource.Play();
            }

            return false;
        }

        public bool IsInteractable(GameObject item)
        {
            var hoe = item.GetComponent<Hoe>();
            if (hoe != null)
            {
                return PlantIsFinished;
            }

            var hammer = item.GetComponent<Hammer>();
            return hammer != null;
        }
    }
}