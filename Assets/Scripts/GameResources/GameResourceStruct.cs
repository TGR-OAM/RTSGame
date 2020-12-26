using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameResources
{
    [Serializable]
    public struct GameResourceStruct
    {
        [SerializeField] public int neoSteel;

        [SerializeField] public int titanium;

        [SerializeField] public int unobtanium;
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

        public static bool operator >=(GameResourceStruct a, GameResourceStruct b)
        {
            if (a.neoSteel >= b.neoSteel && a.titanium >= b.titanium && a.unobtanium >= b.unobtanium) return true;
            return false;
        }
        
        public static bool operator <=(GameResourceStruct a, GameResourceStruct b)
        {
            if (a.neoSteel <= b.neoSteel && a.titanium <= b.titanium && a.unobtanium <= b.unobtanium) return true;
            return false;
        }

        public GameResourceStruct(int neoSteel,int titanium, int unobtanium)
        {
            this.neoSteel = neoSteel;
            this.titanium = titanium;
            this.unobtanium = unobtanium;
        }
    }

    public delegate void VoidWithGameResourceStruct(GameResourceStruct gameResourceStruct);
}
