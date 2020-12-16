using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bots
{
    public class Body: MonoBehaviour
    {
        [SerializeField] public float _hp { get; private set; }
        public byte _numberOfWeapons;
        [SerializeField] public float _mass { get; private set; }
        void Start()
        {

        }
    }
    public enum _Body : int
    {
        Heavy,
        Medium,
        Light
    }
}