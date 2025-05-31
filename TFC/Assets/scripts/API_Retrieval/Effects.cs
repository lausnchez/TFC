using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects
{
    [Serializable]
    public class Effect
    {
        public int id;
        public string name;
        public string description;
        public int active_turns;
        public int value;
    }
}
