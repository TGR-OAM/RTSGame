using System;
using System.Collections.Generic;
using Orders.EntityOrder;
using Units;
using UnitsControlScripts;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu_DemoStartScripts
{
    public static class EntityLoader
    {
        #region OrdersData
        
        public static Dictionary<Type, LoadEntetyData> EntetiesParamsFromStringDisctionary = new Dictionary<Type, LoadEntetyData>();

        public static bool isPlayingGame = false;

        public static void AddNewEntetyTypeToDictionary(Type entetyType, LoadEntetyData entetyInitParams)
        {
            if(!EntetiesParamsFromStringDisctionary.ContainsKey(entetyType))
                EntetiesParamsFromStringDisctionary.Add(entetyType, entetyInitParams);
            else
            {
                EntetiesParamsFromStringDisctionary.Remove(entetyType);
                EntetiesParamsFromStringDisctionary.Add(entetyType, entetyInitParams);
            }
        }
        public static LoadEntetyData GetOrderInitParamsFromDictionary(Type unitType)
        {
            return EntetiesParamsFromStringDisctionary[unitType];
        }
        public static bool Contain(Type entetyType)
        {
            return EntetiesParamsFromStringDisctionary.ContainsKey(entetyType);
        }
        public static void ClearDictionary()
        {
            EntetiesParamsFromStringDisctionary.Clear();
        }

        #endregion
    }

    public class LoadEntetyData
    {
        public GameObject PlayerPrefab;
        public Dictionary<string,GameOrderInitParams> OrderInitParams;

        public LoadEntetyData(GameObject PlayerPrefab , Dictionary<string,GameOrderInitParams> OrderInitParams)
        {
            this.PlayerPrefab = PlayerPrefab;
            this.OrderInitParams = OrderInitParams;
        }

        public GameOrderInitParams GetOrderInitParamsFromType(string typeName)
        {
            return OrderInitParams[typeName];
        }
        
    }
}