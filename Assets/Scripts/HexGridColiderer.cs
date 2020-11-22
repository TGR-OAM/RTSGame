using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HexGridColiderer
    {
        HexGridData MapData;
        List<GameObject> Coliders;
        GameObject ColiderPart;

        float widthInUnits;
        float heightInUnits;

        public HexGridColiderer(HexGrid hexGrid)
        {
            this.MapData = hexGrid.MapData;

            ColiderPart = new GameObject("Colider Part");
            ColiderPart.transform.parent = hexGrid.transform;

            widthInUnits = MapData.width * HexMetrics.innerRadius * 2f * MapData.cellSize + (MapData.height > 1f ? HexMetrics.innerRadius * MapData.cellSize : 0);
            heightInUnits = 1.5f * (MapData.height-1) * MapData.cellSize * HexMetrics.outerRadius + MapData.cellSize * HexMetrics.outerRadius * 2f;

            Coliders = new List<GameObject>();
            ReUpdateAllColiders();
        }

        public void ReUpdateAllColiders()
        {
            foreach(GameObject Colider in Coliders)
            {
                GameObject.Destroy(Colider);
            }
            Coliders = new List<GameObject>();
            List<float> DifferentHeights = new List<float>();
            foreach (float height in MapData.HeightMap)
                if (!DifferentHeights.Contains(height)) DifferentHeights.Add(height);

            foreach (float height in DifferentHeights)
            {
                GameObject PlaneCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
                PlaneCollider.name = "Colider " + height;
                PlaneCollider.transform.parent = ColiderPart.transform;
                PlaneCollider.transform.localPosition = new Vector3(widthInUnits / 2f - MapData.cellSize * HexMetrics.innerRadius, height - .0005f, heightInUnits / 2f - MapData.cellSize * HexMetrics.outerRadius);
                PlaneCollider.transform.localScale = new Vector3(widthInUnits, .001f, heightInUnits);

                GameObject.Destroy(PlaneCollider.GetComponent<MeshFilter>());
                GameObject.Destroy(PlaneCollider.GetComponent<MeshRenderer>());

                Coliders.Add(PlaneCollider);
            }
        }


        public bool TryRaycastHexGrid(out Vector3 HexOutput, Ray rayToCast)
        {
            List<RaycastHit> hits = Physics.RaycastAll(rayToCast,Mathf.Infinity).ToList();
            hits = hits.OrderBy(h => h.distance).ToList();

            List<Vector3> Heights = new List<Vector3>();

            foreach(RaycastHit hit in hits)
            {
                Vector3 HexCoords = HexMetrics.CalcHexCoordXZFromDefault(hit.point, MapData.cellSize);
                HexCoords = new Vector3(Mathf.Clamp(HexCoords.x, 0, MapData.width - 1), 0, Mathf.Clamp(HexCoords.z, 0, MapData.height - 1));
                HexCoords += new Vector3(0, MapData.HeightMap[(int)HexCoords.z * MapData.width + (int)HexCoords.x],0);
                if (Mathf.Approximately(hit.point.y, HexCoords.y))
                {
                    HexOutput = HexCoords;
                    return true;
                }
                else
                {
                    Heights.Add(HexCoords);
                }
            }

            for(int i = 1;i< Heights.Count;i++)
            {
                if (Heights[i].y > Heights[i - 1].y)
                {
                    HexOutput = Heights[i];
                    Debug.Log(HexOutput);
                    return true;
                }
            }

            HexOutput = new Vector3();
            return false;

        }


    }
}
