﻿using System;
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

        

        public HexGridColiderer(HexGrid hexGrid)
        {
            this.MapData = hexGrid.MapData;

            ColiderPart = new GameObject("Colider Part");
            ColiderPart.transform.parent = hexGrid.transform;

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
                PlaneCollider.transform.localPosition = new Vector3(MapData.widthInUnits / 2f - MapData.cellSize * HexMetrics.innerRadius, height - .0005f, MapData.heightInUnits / 2f - MapData.cellSize * HexMetrics.outerRadius);
                PlaneCollider.transform.localScale = new Vector3(MapData.widthInUnits, .001f, MapData.heightInUnits);

                GameObject.Destroy(PlaneCollider.GetComponent<MeshFilter>());
                GameObject.Destroy(PlaneCollider.GetComponent<MeshRenderer>());

                Coliders.Add(PlaneCollider);
            }
        }


        public bool TryRaycastHexGrid(out Vector3 DefOutput, Ray rayToCast)
        {
            Debug.Log("Try ray cast");

            List<RaycastHit> hitsDown = Physics.RaycastAll(rayToCast.origin, rayToCast.direction, Mathf.Infinity).ToList();
            hitsDown = hitsDown.OrderBy(h => h.distance).ToList();

            List<RaycastHit> hitsUp = Physics.RaycastAll(rayToCast.origin, -rayToCast.direction, Mathf.Infinity).ToList();
            hitsUp = hitsUp.OrderByDescending(h => h.distance).ToList();

            hitsUp.AddRange(hitsDown);
            List<RaycastHit> hits = hitsUp;

            Debug.Log(hits.Count);

            List <Vector3> Heights = new List<Vector3>();
            List<float> HeightsOfActualCollision = new List<float>();

            foreach(RaycastHit hit in hits)
            {
                Vector3 HexCoords = HexMetrics.CalcHexCoordXZFromDefault(hit.point, MapData.cellSize);
                HexCoords = new Vector3(Mathf.Clamp(HexCoords.x, 0, MapData.width - 1), 0, Mathf.Clamp(HexCoords.z, 0, MapData.height - 1));
                HexCoords += new Vector3(0, MapData.HeightMap[(int)HexCoords.z * MapData.width + (int)HexCoords.x],0);
                if (Mathf.Approximately(hit.point.y, HexCoords.y))
                {
                    DefOutput = hit.point;
                    return true;
                }
                else
                {
                    Heights.Add(new Vector3(hit.point.x, HexCoords.y, hit.point.z)); ;
                    HeightsOfActualCollision.Add(hit.point.y);
                }
            }

            for(int i = 1;i< Heights.Count;i++)
            {
                if (Heights[i].y > Heights[i - 1].y && HeightsOfActualCollision[i] < Heights[i].y) 
                {
                    DefOutput = Heights[i];
                    return true;
                }
            }

            DefOutput = new Vector3();
            return false;

        }


    }
}
