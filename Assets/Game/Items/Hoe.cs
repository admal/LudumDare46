using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class Hoe : MonoBehaviour, IPickable
    {
        public bool IsPickable => true;

        public void PickDown()
        {
        }
    }
}