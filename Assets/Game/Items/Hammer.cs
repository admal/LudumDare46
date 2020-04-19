using Game.Items;
using Game.Plants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Items
{
    public class Hammer : MonoBehaviour, IPickable
    {
        [SerializeField]
        private float _repairValue = 10f;
        [SerializeField]
        private float _repairRatio = 4f;
        private bool _canUse = true;
        public bool IsPickable => true;

        public void PickDown()
        {
        }

        public void RepairPlant(Plant plant)
        {
            if (_canUse)
            {
                _canUse = false;
                plant.AddHealth(_repairValue);
                StartCoroutine(StartTimer());
            }
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(_repairRatio);
            _canUse = true;
        }
    }
}