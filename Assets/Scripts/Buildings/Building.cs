using System;
using System.Collections.Generic;
using System.Linq;
using GameResources;
using HexWorldinterpretation;
using MainMenu_DemoStartScripts;
using UnitsControlScripts;
using UnityEngine;
using Orders;

namespace Buildings
{
    [RequireComponent(typeof(Collider), typeof(OrderableObject),typeof(DamageSystem))]
    public class Building : MonoBehaviour
    {
        public HexGrid hexGrid;
        public Material Materials;
        public Renderer[] MainRenderers;
        public float MaxHp;
        public float TimeUntilFullConstruction;
        public Vector2Int Size;
        public Vector3 HexCoords;
        
        public bool isStartWithFullHp = false;

        public Vector3 CreationOutput;

        public Collider ObjectCollider;
        public OrderableObject orderableObject;
        public DamageSystem damageSystem;

        public GameResourceStruct BuildingCost;

        public bool isPlaced = false;

        protected void Start()
        {
            BaseBuildingInitialization();
            BaseBuildingOrderInitialization();
        }

        public void SetTransparent(bool available)
        {
            if (available)
            {
                foreach (var VARIABLE in MainRenderers)
                {
                    VARIABLE.material.color = Color.green;
                }

            }
            else
            {
                foreach (var VARIABLE in MainRenderers)
                {
                    VARIABLE.material.color = Color.red;
                }
            }

        }
        public void SetNormal()
        {
            foreach (var VARIABLE in MainRenderers)
            {
                VARIABLE.material.color = Color.white;
            }
            
            CreationOutput = this.transform.position;
            damageSystem.TrySetActiveHealthBar(true);
            damageSystem.TryUpdateHealthBar();
            isPlaced = true;

        }
        private void OnDrawGizmos()
        {
           if(!Application.isPlaying) return;
            
            for (int x = 0; x < Size.x; x++)
            {
                for (int z = 0; z < Size.y; z++)
                {
                    Mesh hexMesh = new Mesh();
                    hexMesh.vertices = HexMetrics.corners;
                    int[] tris = { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
                    hexMesh.triangles = tris;
                    hexMesh.RecalculateNormals();
                    Gizmos.color = Color.Lerp(Color.red, Color.green, damageSystem.Hp/damageSystem.MaxHp);

                    Vector3 CenterCoord = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(HexCoords, hexGrid.MapData) + new Vector3(HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z), hexGrid.MapData).x, 0, HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z), hexGrid.MapData).z);

                    Gizmos.DrawMesh(hexMesh, 0, CenterCoord, Quaternion.identity, Vector3.one * hexGrid.MapData.cellSize);
                }
            }
        }

        protected void BaseBuildingInitialization()
        {
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.TrySetActiveHealthBar(false);
            if(isStartWithFullHp) damageSystem.Heal(MaxHp);
            CreationOutput = this.transform.position;
            
        }

        protected void BaseBuildingOrderInitialization()
        {
            if (EntityLoader.Contain(this.GetType()))
            {
                orderableObject.SetPossibleOrderTypes(EntityLoader.GetOrderInitParamsFromDictionary(this.GetType())
                    .OrderInitParams.Values.ToList());
            }
            
        }
    }
}