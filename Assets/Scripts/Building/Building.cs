using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Building : MonoBehaviour
{
    public HexGrid hexGrid;
    public Material Materials;
    public Renderer[] MainRenderer;
    public Vector2Int Size;
    public Vector3 HexCoords;
    public void SetTransparent(bool available)
    {
        if (available)
        {
            foreach (var VARIABLE in MainRenderer)
            {
                VARIABLE.material.color = Color.green;
            }

        }
        else
        {
            foreach (var VARIABLE in MainRenderer)
            {
                VARIABLE.material.color = Color.red;
            }
        }

    }

    public void SetNormal()
    {
        foreach (var VARIABLE in MainRenderer)
        {
            VARIABLE.material.color = Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int z = 0; z < Size.y; z++)
            {
                Mesh hexMesh = new Mesh();
                hexMesh.vertices = HexMetrics.corners;
                int[] tris = { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
                hexMesh.triangles = tris;
                hexMesh.RecalculateNormals();
                Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);

                Vector3 CenterCoord = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(HexCoords, hexGrid.MapData) + new Vector3(HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z), hexGrid.MapData).x, 0, HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z), hexGrid.MapData).z);

                Gizmos.DrawMesh(hexMesh, 0, CenterCoord, Quaternion.identity, Vector3.one * hexGrid.MapData.cellSize);
            }
        }
        
    }
}