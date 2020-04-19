using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Items
{
    public class InteractionMarker : MonoBehaviour
    {
        [SerializeField]
        private GameObject _marker;

        private void Start()
        {
            HideMarker();
        }

        public void ShowMarker()
        {
            _marker.SetActive(true);
        }

        public void HideMarker()
        {
            _marker.SetActive(false);
        }
    }
}