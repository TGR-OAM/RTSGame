using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace HexWorldinterpretation
{
    public class HexGridRenderer
    {
        HexGridData MapData;

        HexColumnRenderer[] hexColumnRenderers;

        public HexGridRenderer(HexGrid hexGrid)
        {
            this.MapData = hexGrid.MapData;
            hexColumnRenderers = new HexColumnRenderer[MapData.width];

            GameObject MeshPart = new GameObject("Mesh Part", typeof(NavMeshSurface));

            MeshPart.transform.parent = hexGrid.transform;

            for (int x = 0;x < MapData.width;x++)
            {
                InitColumn(x, hexGrid, MeshPart.transform);
            }
            
            NavMeshSurface[] navMeshSurfaces = MeshPart.GetComponents(typeof(NavMeshSurface)).Select(x => x as NavMeshSurface).ToArray();
            
            foreach (NavMeshSurface navMeshSurface in navMeshSurfaces)
            {
                navMeshSurface.BuildNavMesh();
            }
        }

        public void UpdateHexGridMesh(List<Vector3> HexagonsToUpdate)
        {
            List<HexColumnRenderer> ColumsToUpdate = new List<HexColumnRenderer>();
            List<int> XCoordColumsToUpdate = new List<int>();

            foreach(Vector3 HexagonCoord in HexagonsToUpdate)
            {
                if (HexagonCoord.x != 0)
                {
                    if (!XCoordColumsToUpdate.Contains((int)HexagonCoord.x - 1))
                    {
                        ColumsToUpdate.Add(hexColumnRenderers[(int)HexagonCoord.x - 1]);
                        XCoordColumsToUpdate.Add((int)HexagonCoord.x - 1);
                    }
                }
                if (HexagonCoord.x != hexColumnRenderers.Length - 1)
                {
                    if (!XCoordColumsToUpdate.Contains((int)HexagonCoord.x + 1))
                    {
                        ColumsToUpdate.Add(hexColumnRenderers[(int)HexagonCoord.x + 1]);
                        XCoordColumsToUpdate.Add((int)HexagonCoord.x + 1);
                    }
                }
                if (!XCoordColumsToUpdate.Contains((int)HexagonCoord.x ))
                {
                    ColumsToUpdate.Add(hexColumnRenderers[(int)HexagonCoord.x ]);
                    XCoordColumsToUpdate.Add((int)HexagonCoord.x);
                }
            }

            for (int i = 0; i < ColumsToUpdate.Count; i++)
            {
                ColumsToUpdate[i].UpdateColumn(XCoordColumsToUpdate[i]);
            }
        }

        void InitColumn(int x, HexGrid hexGrid, Transform MeshPart)
        {
            hexColumnRenderers[x] = new HexColumnRenderer(x, hexGrid, MeshPart);
        }

    }
}
