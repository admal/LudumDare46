using Game.Plants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class WateringCan : MonoBehaviour, IPickable
    {
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _waterSound;
        [SerializeField]
        private float _waterValue = 20f;
        [SerializeField]
        private float _useRatio = 2f;
        private bool _canUse = true;
        public bool CanUse => _canUse;
        public bool IsPickable => true;

        public void PickDown()
        {
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void WaterPlant(PlantWaterController plant)
        {
            if (_canUse)
            {
                _audioSource.clip = _waterSound;
                _audioSource.Play();
                _canUse = false;
                plant.ChangeWater(_waterValue);
                StartCoroutine(StartTimer());
            }
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(_useRatio);
            _canUse = true;
        }
    }
}