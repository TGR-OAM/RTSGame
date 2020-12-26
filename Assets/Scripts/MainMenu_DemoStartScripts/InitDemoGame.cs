using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using GameResources;
using MainMenu_DemoStartScripts;
using Orders.EntityOrder;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InitDemoGame : MonoBehaviour
{
    public int DemoSceneIndex;
    
    public Unit[] UnitsToLoadHub;
    public Unit[] UnitsToLoadBarracks;
    public Unit[] UnitsToLoadConstructor;

    public Building[] BuildingsToLoad;

    public void StartDemoGame()
    {
        #region OrdersExamples

        MoveOrderInitParams MoveExample = new MoveOrderInitParams("Move");
        AttackOrderInitParams AttackExample = new AttackOrderInitParams("Attack");
        MoveAttackOrderInitParams MoveAttackExample = new MoveAttackOrderInitParams("Move and attack");
        DefendOrderInitParams DefendExample = new DefendOrderInitParams("Defend");

        Dictionary<string,BuildOrderInitParams> BuildExamples = new Dictionary<string,BuildOrderInitParams>();

        foreach (Building var in BuildingsToLoad)
        {
            if (var is MainHub) continue;
            BuildExamples.Add(typeof(BuildOrderInitParams).FullName + "."+ var.name,new BuildOrderInitParams(var, "Build "+var.name));
        }

        Dictionary<string, UnitCreationOrderInitParams> UnitCreationHubExamples =
            new Dictionary<string, UnitCreationOrderInitParams>();

        foreach (Unit unit in UnitsToLoadHub)
        {
            UnitCreationHubExamples.Add(typeof(UnitCreationOrderInitParams).FullName + "." + unit.name, new UnitCreationOrderInitParams(unit, 1f, "Create unit " + unit.name));
        }
        
        Dictionary<string, UnitCreationOrderInitParams> UnitCreationBarracksExamples =
            new Dictionary<string, UnitCreationOrderInitParams>();

        foreach (Unit unit in UnitsToLoadBarracks)
        {
            UnitCreationBarracksExamples.Add(typeof(UnitCreationOrderInitParams).FullName + "." + unit.name, new UnitCreationOrderInitParams(unit, 1f, "Create unit " + unit.name));
        }
        
        Dictionary<string, UnitCreationOrderInitParams> UnitCreationConstructorExamples =
            new Dictionary<string, UnitCreationOrderInitParams>();

        foreach (Unit unit in UnitsToLoadConstructor)
        {
            UnitCreationConstructorExamples.Add(typeof(UnitCreationOrderInitParams).FullName + "." + unit.name, new UnitCreationOrderInitParams(unit, 1f, "Create unit " + unit.name));
        }

        #endregion
        
        EntityLoader.ClearDictionary();

        #region UnitLoadInitialization

        List<Unit> UnitsToLoad = UnitsToLoadHub.ToList();
        UnitsToLoad.AddRange(UnitsToLoadBarracks);
        UnitsToLoad.AddRange(UnitsToLoadConstructor);
        
        foreach (Unit unit in UnitsToLoad)
        {
            if (unit is Robot)
            {
                Debug.Log(unit);
                LoadEntetyData warrioEntetyData = new LoadEntetyData(unit.gameObject,new Dictionary<string, GameOrderInitParams>
                {
                    {MoveExample.ToString() ,MoveExample},
                    {AttackExample.ToString() ,AttackExample},
                    {MoveAttackExample.ToString() ,MoveAttackExample},
                    {DefendExample.ToString() , DefendExample}
                });
                
                EntityLoader.AddNewEntetyTypeToDictionary(unit.GetType(), warrioEntetyData);
            }
            else if (unit is Warrior)
            {
                LoadEntetyData warrioEntetyData = new LoadEntetyData(unit.gameObject,new Dictionary<string, GameOrderInitParams>
                {
                    {MoveExample.ToString() ,MoveExample},
                    {AttackExample.ToString() ,AttackExample},
                    {MoveAttackExample.ToString() ,MoveAttackExample},
                    {DefendExample.ToString() , DefendExample}
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
        
        #region Barracks Load
        Dictionary<string, GameOrderInitParams> AllOrdersExamplesBarracks = new Dictionary<string, GameOrderInitParams>();
                
        foreach (KeyValuePair<string,UnitCreationOrderInitParams> keyValuePair in UnitCreationBarracksExamples)
        {
            AllOrdersExamplesBarracks.Add(keyValuePair.Key,keyValuePair.Value);
        }
                
        LoadEntetyData BarracksLoadData = new LoadEntetyData(BuildingsToLoad[0].gameObject, AllOrdersExamplesBarracks);
                
        EntityLoader.AddNewEntetyTypeToDictionary(BuildingsToLoad[0].GetType(),BarracksLoadData);
        #endregion

        #region HubLoad

        Dictionary<string, GameOrderInitParams> AllOrdersExamplesHub = new Dictionary<string, GameOrderInitParams>();
                
        foreach (KeyValuePair<string,UnitCreationOrderInitParams> keyValuePair in UnitCreationHubExamples)
        {
            AllOrdersExamplesHub.Add(keyValuePair.Key,keyValuePair.Value);
        }
                
        LoadEntetyData HubLoadData = new LoadEntetyData(BuildingsToLoad[2].gameObject, AllOrdersExamplesHub);
                
        EntityLoader.AddNewEntetyTypeToDictionary(BuildingsToLoad[2].GetType(),HubLoadData);

        #endregion
        
        #region Constructor Load

        Dictionary<string, GameOrderInitParams> AllOrdersExamplesConstructor = new Dictionary<string, GameOrderInitParams>();
                
        foreach (KeyValuePair<string,UnitCreationOrderInitParams> keyValuePair in UnitCreationConstructorExamples)
        {
            AllOrdersExamplesConstructor.Add(keyValuePair.Key,keyValuePair.Value);
        }
                
        LoadEntetyData ConstructorLoadData = new LoadEntetyData(BuildingsToLoad[1].gameObject, AllOrdersExamplesConstructor);
                
        EntityLoader.AddNewEntetyTypeToDictionary(BuildingsToLoad[1].GetType(),ConstructorLoadData);

        #endregion
        

        #endregion
        
        SceneManager.LoadScene(DemoSceneIndex);
    }
}