using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class Billboard : MonoBehaviour
    {
        public Transform cam;

        private void Start()
        {
            cam = Camera.main.transform; //TODO: fix it
        }
        private void Update()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}