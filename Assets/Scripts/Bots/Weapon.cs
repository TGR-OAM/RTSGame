using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bots
{
    public struct Weapon
    {
        [SerializeField] public float _damage { get; private set; }
        [SerializeField] public float _mass { get; private set; }

    }
    public enum _Weapon : int
    {
        Heavy,
        Medium,
        Light
    }
}