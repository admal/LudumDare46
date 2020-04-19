using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Plants
{
    public class PlantsManager : MonoBehaviour
    {
        public static PlantsManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Plant[] GetPlants()
        {
            return GetComponentsInChildren<Plant>();
        }
    }
}