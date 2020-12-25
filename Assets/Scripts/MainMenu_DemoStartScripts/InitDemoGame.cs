using System.Collections;
using System.Collections.Generic;
using Buildings;
using MainMenu_DemoStartScripts;
using Orders.EntityOrder;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InitDemoGame : MonoBehaviour
{
    public int DemoSceneIndex;

    public Unit[] UnitsToLoad;

    public Building[] BuildingsToLoad;

    public void StartDemoGame()
    {
        #region OrdersExamples

        MoveOrderInitParams MoveExample = new MoveOrderInitParams("Move");
        AttackOrderInitParams AttackExample = new AttackOrderInitParams("Attack");
        MoveAttackOrderInitParams MoveAttackExample = new MoveAttackOrderInitParams("Move and attack");

        Dictionary<string,BuildOrderInitParams> BuildExamples = new Dictionary<string,BuildOrderInitParams>();

        foreach (Building var in BuildingsToLoad)
        {
            if (var is MainHub) continue;
            BuildExamples.Add(typeof(BuildOrderInitParams).FullName + "."+ var.name,new BuildOrderInitParams(var, "Build "+var.name));
        }

        Dictionary<string, UnitCreationOrderInitParams> UnitCreationExamples =
            new Dictionary<string, UnitCreationOrderInitParams>();

        foreach (Unit unit in UnitsToLoad)
        {
            UnitCreationExamples.Add(typeof(UnitCreationOrderInitParams).FullName + "." + unit.name, new UnitCreationOrderInitParams(unit, 1f, "Create unit " + unit.name));
        }

        #endregion
        
        EntityLoader.ClearDictionary();

        #region UnitLoadInitialization

        foreach (Unit unit in UnitsToLoad)
        {
            if (unit is Warrior)
            {
                LoadEntetyData warrioEntetyData = new LoadEntetyData(unit.gameObject,new Dictionary<string, GameOrderInitParams>
                {
                    {MoveExample.ToString() ,MoveExample},
                    {AttackExample.ToString() ,AttackExample},
                    {MoveAttackExample.ToString() ,MoveAttackExample},
                });
                
                EntityLoader.AddNewEntetyTypeToDictionary(unit.GetType(), warrioEntetyData);
            }
            else if (unit is Robot)
            {
                Debug.Log(unit);
                LoadEntetyData warrioEntetyData = new LoadEntetyData(unit.gameObject,new Dictionary<string, GameOrderInitParams>
                {
                    {MoveExample.ToString() ,MoveExample},
                    {AttackExample.ToString() ,AttackExample},
                    {MoveAttackExample.ToString() ,MoveAttackExample},
                });
                
                EntityLoader.AddNewEntetyTypeToDictionary(unit.GetType(), warrioEntetyData);
            }
            else if (unit is UnitBuilder)
            {
                Dictionary<string, GameOrderInitParams> AllOrdersExamples = new Dictionary<string, GameOrderInitParams>();
                foreach (KeyValuePair<string,BuildOrderInitParams> keyValuePair in BuildExamples)
                {
                    AllOrdersExamples.Add(keyValuePair.Key,keyValuePair.Value);
                }
                
                AllOrdersExamples.Add(MoveExample.ToString(),MoveExample);

                LoadEntetyData builderEntetyData =
                    new LoadEntetyData(unit.gameObject, AllOrdersExamples);

                EntityLoader.AddNewEntetyTypeToDictionary(unit.GetType(), builderEntetyData);
            }
            else
            {
                LoadEntetyData builderEntetyData =
                    new LoadEntetyData(unit.gameObject, new Dictionary<string, GameOrderInitParams>
                    {
                        {MoveExample.ToString() ,MoveExample}
                    });

                EntityLoader.AddNewEntetyTypeToDictionary(unit.GetType(), builderEntetyData);
            }

        }

        #endregion

        #region BuildingLoadInitialization

        foreach (Building building in BuildingsToLoad)
        {
            if (building is MainHub)
            {
                Dictionary<string, GameOrderInitParams> AllOrdersExamples = new Dictionary<string, GameOrderInitParams>();
                
                foreach (KeyValuePair<string,UnitCreationOrderInitParams> keyValuePair in UnitCreationExamples)
                {
                    AllOrdersExamples.Add(keyValuePair.Key,keyValuePair.Value);
                }
                
                LoadEntetyData HubLoadData = new LoadEntetyData(building.gameObject, AllOrdersExamples);
                
                EntityLoader.AddNewEntetyTypeToDictionary(building.GetType(),HubLoadData);
            }
        }

        #endregion
        
        SceneManager.LoadScene(DemoSceneIndex);
    }
}