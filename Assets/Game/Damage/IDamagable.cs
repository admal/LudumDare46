using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Damage
{
    public interface IDamagable
    {
        bool ApplyDamage(DamageInfo damage);
    }

    public class DamageInfo
    {
        public float BaseDamage;
    }
}
