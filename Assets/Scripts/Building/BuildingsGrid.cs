using Assets.Scripts;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public HexGrid hexGrid;

    private Building flyingBuilding;

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
        flyingBuilding.hexGrid = hexGrid;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (hexGrid.TryRaycastHexGrid(ray,out Vector3 worldPosition))
            {
                Vector3 CoordsOfCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(HexMetrics.CalcHexCoordXZFromDefault(worldPosition, hexGrid.MapData.cellSize),hexGrid.MapData);
                bool available = true;

                available = IsPossibleToBuild(flyingBuilding);

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
        }
    }

    private bool IsPossibleToBuild(Building building)
    {
        Collider thisColider = building.GetComponent<Collider>();
        foreach (Building AlreadyBuilded in hexGrid.MapData.Buildings)
        {
            if (thisColider.bounds.Intersects(AlreadyBuilded.GetComponent<Collider>().bounds))
                return false;
        }
        return true;
    }

    private void PlaceFlyingBuilding()
    {
        flyingBuilding.SetNormal();
        flyingBuilding.gameObject.GetComponentInChildren<Renderer>().material = flyingBuilding.Materials;
        hexGrid.MapData.Buildings.Add(flyingBuilding);
        flyingBuilding.gameObject.layer = 9;
        flyingBuilding = null;
    }
}