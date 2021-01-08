using System;
using HexWorldInterpretation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Buildings
{
    public class Builder 
    {
        public  HexGrid hexGrid;
        private PlayerManager PlayerManager;

        public Building flyingBuilding;

        public bool isAvailable = false;

        public Builder(HexGrid hexGrid, PlayerManager playerManager)
        {
            this.hexGrid = hexGrid;
            PlayerManager = playerManager;
        }
        public void StartPlacingBuilding(Building buildingPrefab)
        {
            if (flyingBuilding != null)
            {
                GameObject.Destroy(flyingBuilding.gameObject);
            }

            flyingBuilding = GameObject.Instantiate(buildingPrefab);
            flyingBuilding.hexGrid = hexGrid;
        }

        public void StopPlacingBuilding()
        {
            if (flyingBuilding != null)
            {
                GameObject.Destroy(flyingBuilding.gameObject);
            }
        }

        public void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (hexGrid.TryRaycastHexGrid(ray, out Vector3 worldPosition) && flyingBuilding!=null)
            {
                flyingBuilding.gameObject.SetActive(true);
                Vector3 CoordsOfCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(HexMetrics.CalcHexCoordXZFromDefault(worldPosition, hexGrid.MapData.cellSize), hexGrid.MapData);

                isAvailable = IsPossibleToBuild(flyingBuilding, HexMetrics.CalcHexCoordXZFromDefault(CoordsOfCenter, hexGrid.MapData.cellSize));

                #region syncing data with building
                flyingBuilding.HexCoords = HexMetrics.CalcHexCoordXZFromDefault(CoordsOfCenter, hexGrid.MapData.cellSize);
                flyingBuilding.transform.position = CoordsOfCenter;
                flyingBuilding.SetTransparent(isAvailable);
                #endregion
            }
        }

        private bool IsPossibleToBuild(Building thisBuilding, Vector3 HexCoord)
        {
            if (IsIntersectingOtherBuilding(thisBuilding))
                return false;
            if (!IsStayingOnSurface(thisBuilding, HexCoord))
                return false;
            if (PlayerManager.PlayerResoucesManager.playerResources <= thisBuilding.BuildingCost)
                return false;

            return true;
        }

        private bool IsIntersectingOtherBuilding(Building thisBuilding)
        {
            Collider thisColider = thisBuilding.GetComponent<Collider>();
            foreach (Building AlreadyBuilded in hexGrid.MapData.ConstructedBuildings)
            {
                if (AlreadyBuilded == null) continue;
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
                    try
                    {
                        if (HexCoord.z % 2 == 0)
                        {
                            if (thisBuilding.transform.position.y !=
                                hexGrid.MapData.HeightMap[
                                    (int) (HexCoord.z + z) * hexGrid.MapData.width + (int) (HexCoord.x + x)])
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (thisBuilding.transform.position.y !=
                                hexGrid.MapData.HeightMap[
                                    (int) (HexCoord.z + z) * hexGrid.MapData.width + (int) (HexCoord.x + x)])
                            {
                                Debug.Log(hexGrid.MapData.HeightMap[
                                    (int) (HexCoord.z + z) * hexGrid.MapData.width + (int) (HexCoord.x + x + z % 2)]);
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CanPlaceFlyingBuilding()
        {
            return isAvailable;
        }

        public void PlaceFlyingBuilding()
        {
            flyingBuilding.SetNormal();
            flyingBuilding.gameObject.GetComponentInChildren<Renderer>().material = flyingBuilding.Materials;
            flyingBuilding.gameObject.layer = 10;
            PlayerManager.PlayerResoucesManager.playerResources -= flyingBuilding.BuildingCost;
            hexGrid.MapData.ConstructedBuildings.Add(flyingBuilding);
            flyingBuilding = null;
            StopPlacingBuilding();
        }
    }
}