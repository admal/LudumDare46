﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public interface IPickable
    {
        bool IsPickable { get; }
        void PickDown();
    }
}