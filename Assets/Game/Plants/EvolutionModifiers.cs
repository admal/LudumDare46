using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Plants
{
    public class EvolutionModifiers : MonoBehaviour
    {
        [SerializeField]
        private float BaseEvolutionValue = 10f;

        public float WaterMultiplier = 1f;
        public float HealthMultipier = 1f;

        public float GetEvolutionChange()
        {
            if (WaterMultiplier == 0)
            {
                return 0;
            }

            var multiplier = WaterMultiplier + HealthMultipier;
            return BaseEvolutionValue * multiplier;
        }
    }
}