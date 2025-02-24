using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LauraJoshua
{
    public class Card: ScriptableObject
    {
        public string cardName;
        public int mana;
        public int minDamage;
        public int maxDamage;
        public DamageType damageType;

        public enum DamageType
        {
            Physical,
            Magical,

        }

    }
}

