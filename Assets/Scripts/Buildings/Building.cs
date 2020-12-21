using System;
using System.Collections.Generic;
using HexWorldinterpretation;
using UnitsControlScripts;
using UnityEngine;

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
        public Collider ObjectCollider;
        public bool isStartWithFullHp = false;
        public float timeUntilConstruction;

        public OrderableObject orderableObject { get; private set; }
        public DamageSystem damageSystem { get; private set; }

        private void Start()
        {
            BaseBuildingInitialization();
            orderableObject.SetPossibleOrderTypes(new List<Type> {});
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
            orderableObject = this.GetComponent<OrderableObject>();
            damageSystem = this.GetComponent<DamageSystem>();
            ObjectCollider = this.GetComponent<Collider>();
            damageSystem.SetMaxHpd(MaxHp);
            if(isStartWithFullHp) damageSystem.Heal(MaxHp);
        }
    }
}