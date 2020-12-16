using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bots
{
    public class BotParts : MonoBehaviour
    {
        [SerializeField]public List<GameObject> _Bodies { get; private set; }
        public List<GameObject> _Weapons { get; private set; }
        public List<GameObject> _Wheels { get; private set; }

    }

    [CustomEditor(typeof(BotParts))]
    public class BotPartsEditor : Editor
    {
        private BotParts _botParts;
        private void OnEnable()
        {
            _botParts = (BotParts) target;
        }
    
    }
}