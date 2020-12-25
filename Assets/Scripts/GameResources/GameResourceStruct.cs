using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameResources
{
    [System.Serializable]
    public struct GameResourceStruct
    {
        [SerializeField]
        public int neoSteel { get; set; }
        [SerializeField]
        public int titanium { get; set; }
        [SerializeField]
        public int unobtanium { get; set; }
        public static GameResourceStruct operator +(GameResourceStruct a, GameResourceStruct b)
        {
            GameResourceStruct result = new GameResourceStruct();
            result.neoSteel = a.neoSteel + b.neoSteel;
            result.titanium = a.titanium + b.titanium;
            result.unobtanium = a.unobtanium + b.unobtanium;
            return result;
        }
        public static GameResourceStruct operator -(GameResourceStruct a, GameResourceStruct b)
        {
            GameResourceStruct result = new GameResourceStruct();
            result.neoSteel = a.neoSteel - b.neoSteel;
            result.titanium = a.titanium - b.titanium;
            result.unobtanium = a.unobtanium - b.unobtanium;
            return result;
        }
    }
}
