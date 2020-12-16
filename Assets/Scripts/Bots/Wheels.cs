using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bots
{
    public struct Wheels
    { 
        public float _speed { get; private set; }
    }
    public enum _Wheels : int
    {
        Heavy,
        Medium,
        Light
    }
}