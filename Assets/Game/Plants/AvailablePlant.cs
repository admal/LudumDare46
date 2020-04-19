using Game.Plants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Plants
{
    public class AvailablePlant : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject item)
        {
            var plant = item.GetComponent<Plant>();
            if (plant != null)
            {
                plant.transform.SetParent(this.transform.parent);
                plant.transform.position = this.transform.position;
                plant.PlantThePlant(this);

                this.gameObject.SetActive(false);
                Debug.Log("Planted!");

                return true;
            }

            Debug.Log("Not planted!");
            return false;
        }

        public bool IsInteractable(GameObject item)
        {
            var plant = item.GetComponent<Plant>();
            return plant != null;
        }
    }
}