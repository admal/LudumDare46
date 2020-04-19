using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class DummyItem : MonoBehaviour, IPickable
    {
        public bool IsPickable { get; set; } = true;

        public void PickDown()
        {
        }
    }
}