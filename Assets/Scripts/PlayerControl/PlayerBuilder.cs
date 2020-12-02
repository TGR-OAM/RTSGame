using Assets.Scripts.Buildings;
using Assets.Scripts.HexWorldinterpretation;
using UnityEngine;

namespace Assets.Scripts.PlayerControl
{
    public class PlayerBuilder : MonoBehaviour
    {
        public  HexGrid hexGrid;

        private Building flyingBuilding;


        public void StartPlacingBuilding(Building buildingPrefab)
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
            }

            flyingBuilding = Instantiate(buildingPrefab);
            flyingBuilding.hexGrid = hexGrid;
            Debug.Log("Started to buils ");
        }

        void StopPlacingBuilding()
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
            }
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (hexGrid.TryRaycastHexGrid(ray, out Vector3 worldPosition) && flyingBuilding!=null)
            {
                flyingBuilding.gameObject.SetActive(true);
                Vector3 CoordsOfCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(HexMetrics.CalcHexCoordXZFromDefault(worldPosition, hexGrid.MapData.cellSize), hexGrid.MapData);
                bool available = true;

                available = IsPossibleToBuild(flyingBuilding, HexMetrics.CalcHexCoordXZFromDefault(CoordsOfCenter, hexGrid.MapData.cellSize));

                #region syncing data with building
                flyingBuilding.HexCoords = HexMetrics.CalcHexCoordXZFromDefault(CoordsOfCenter, hexGrid.MapData.cellSize);
                flyingBuilding.transform.position = CoordsOfCenter;
                flyingBuilding.SetTransparent(available);
                #endregion

                if (available && Input.GetMouseButtonDown(0))
                {
                    Debug.Log(CoordsOfCenter);
                    PlaceFlyingBuilding();
                }

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopPlacingBuilding();
            }
        }

        private bool IsPossibleToBuild(Building thisBuilding, Vector3 HexCoord)
        {
            if (IsIntersectingOtherBuilding(thisBuilding))
                return false;
            if (!IsStayingOnSurface(thisBuilding, HexCoord))
                return false;


            return true;
        }

        private bool IsIntersectingOtherBuilding(Building thisBuilding)
        {
            Collider thisColider = thisBuilding.GetComponent<Collider>();
            foreach (Building AlreadyBuilded in hexGrid.MapData.ConstructedBuildings)
            {
                if (thisColider.bounds.Intersects(AlreadyBuilded.GetComponent<Collider>().bounds))
                    return true;
            }
            return false;
        }

        private bool IsStayingOnSurface(Building thisBuilding, Vector3 HexCoord)
        {
            for(int x = 0;x< thisBuilding.Size.x;x++)
            {
                for (int z = 0; z < thisBuilding.Size.y; z++)
                {
                    if (HexCoord.z % 2 == 0)
                    {
                        if (thisBuilding.transform.position.y != hexGrid.MapData.HeightMap[(int)(HexCoord.z + z) * hexGrid.MapData.width + (int)(HexCoord.x + x)])
                        { 
                            return false;
                        }
                    }
                    else
                    {
                        if (thisBuilding.transform.position.y != hexGrid.MapData.HeightMap[(int)(HexCoord.z + z) * hexGrid.MapData.width + (int)(HexCoord.x + x)])
                        {
                            Debug.Log(hexGrid.MapData.HeightMap[(int)(HexCoord.z + z) * hexGrid.MapData.width + (int)(HexCoord.x + x + z % 2)]);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void PlaceFlyingBuilding()
        {
            flyingBuilding.SetNormal();
            flyingBuilding.gameObject.GetComponentInChildren<Renderer>().material = flyingBuilding.Materials;
            hexGrid.MapData.ConstructedBuildings.Add(flyingBuilding);
            flyingBuilding.gameObject.layer = 9;
            flyingBuilding = null;
            StopPlacingBuilding();
        }
    }
}