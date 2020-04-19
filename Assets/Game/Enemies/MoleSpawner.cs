using Game.Plants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    [RequireComponent(typeof(Spawner))]
    public class MoleSpawner : MonoBehaviour
    {
        private Spawner _spawner;
        private void Start()
        {
            _spawner = GetComponent<Spawner>();
            Plants.Plant.OnPlanted += OnPlanted;
        }

        private void OnPlanted(Plant obj)
        {
            _spawner.StartSpawning();
            Plants.Plant.OnPlanted -= OnPlanted;
        }
    }
}