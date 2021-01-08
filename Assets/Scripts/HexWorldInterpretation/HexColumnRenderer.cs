using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexWorldInterpretation
{
    public class HexColumnRenderer
    {
        HexGridData MapData;
        GameObject Column;

        public HexColumnRenderer(int x,HexGrid hexGrid, Transform MeshPart)
        {
            this.MapData = hexGrid.MapData;

            Column = new GameObject("Column " + x, typeof(MeshRenderer),typeof(MeshFilter));
            Column.transform.parent = MeshPart;

            UpdateColumn(x);
        }

        public void UpdateColumn(int x)
        {
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector3> Uvs = new List<Vector3>();
            List<int> Tris = new List<int>();

            float HexSizeWithPadding = MapData.cellSize - MapData.cellPadding;

            for (int z = 0; z < MapData.height; z++)
            {
                InitCell(Vertices, Uvs, Tris, x, z, HexSizeWithPadding);
            }
            
            Mesh ColumnMesh = new Mesh();
            
            ColumnMesh.vertices = Vertices.ToArray();
            ColumnMesh.SetUVs(2,Uvs);
            ColumnMesh.SetTriangles(Tris,0);

            ColumnMesh.RecalculateNormals();
            ColumnMesh.Optimize();

            Column.layer = 9;
            Column.GetComponent<MeshFilter>().mesh = ColumnMesh;
            Column.GetComponent<MeshRenderer>().material = MapData.Default;
        }


        void InitCell(List<Vector3> Vertices,List<Vector3> Uv,List<int> Tris, int x, int z,float HexSizeWithPadding)
        {
            Vector3 ThisCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z), MapData);

            Color thisColor = MapData.ColorMap[z * MapData.width + x];
            int thisTerrainType = MapData.TerrainTypeMap[z * MapData.width + x];

            #region init hexagon inside

            for (int i = 2; i < HexMetrics.corners.Count(); i++)
            {
                AddTriangle(Vertices, Uv, Tris, HexMetrics.corners[0] * HexSizeWithPadding + ThisCenter,
                    HexMetrics.corners[i - 1] * HexSizeWithPadding + ThisCenter,
                    HexMetrics.corners[i] * HexSizeWithPadding + ThisCenter, thisTerrainType);
            }

            #endregion

            #region init up in one column padding

            if (z != MapData.height - 1)
            {
                Vector3 OtherCenterInThisColumn =
                    HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z + 1), MapData);

                Color OtherCenterInThisColumnColor = MapData.ColorMap[(z + 1) * MapData.width + x];
                int otherCenterInThisColumnTerrainType = MapData.TerrainTypeMap[(z + 1) * MapData.width + x];

                if (z % 2 == 0)
                {
                    AddSquare(Vertices, Uv, Tris, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding,
                        OtherCenterInThisColumn + HexMetrics.corners[4] * HexSizeWithPadding,
                        OtherCenterInThisColumn + HexMetrics.corners[3] * HexSizeWithPadding,
                        ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding,
                        thisTerrainType, otherCenterInThisColumnTerrainType,
                        otherCenterInThisColumnTerrainType, thisTerrainType);
                }
                else
                {
                    AddSquare(Vertices, Uv, Tris, ThisCenter + HexMetrics.corners[5] * HexSizeWithPadding,
                        OtherCenterInThisColumn + HexMetrics.corners[3] * HexSizeWithPadding,
                        OtherCenterInThisColumn + HexMetrics.corners[2] * HexSizeWithPadding,
                        ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding,
                        thisTerrainType, otherCenterInThisColumnTerrainType,
                        otherCenterInThisColumnTerrainType, thisTerrainType);
                }
            }

            #endregion

            #region init right padding

            Vector3 RightCenter = new Vector3();
            Color RightColor = new Color();
            int RightTerrainType = 0;

            if (x != MapData.width - 1)
            {
                RightCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z), MapData);
                RightColor = MapData.ColorMap[z * MapData.width + (x + 1)];
                RightTerrainType = MapData.TerrainTypeMap[z * MapData.width + (x + 1)];
                AddSquare(Vertices, Uv, Tris, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding,
                    RightCenter + HexMetrics.corners[5] * HexSizeWithPadding,
                    RightCenter + HexMetrics.corners[4] * HexSizeWithPadding,
                    ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding, thisTerrainType,
                    RightTerrainType, RightTerrainType, thisTerrainType);
            }

            #endregion

            #region init right up padding

            if (z % 2 == 1 && z != MapData.height - 1 && x != MapData.width - 1)
            {
                Vector3 RightUpCenter =
                    HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z + 1), MapData);
                Color RightUpColor = MapData.ColorMap[(z + 1) * MapData.width + (x + 1)];
                int RightUpTerrainType = MapData.TerrainTypeMap[(z + 1) * MapData.width + (x + 1)];
                AddSquare(Vertices, Uv, Tris, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding,
                    RightUpCenter + HexMetrics.corners[4] * HexSizeWithPadding,
                    RightUpCenter + HexMetrics.corners[3] * HexSizeWithPadding,
                    ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding, thisTerrainType,
                    RightUpTerrainType, RightUpTerrainType, thisTerrainType);
            }

            #endregion

            #region init right down padding

            if (z % 2 == 1 && z != 0 && x != MapData.width - 1)
            {
                Vector3 RightDownCenter =
                    HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z - 1), MapData);
                Color RightDownColor = MapData.ColorMap[(z - 1) * MapData.width + (x + 1)];
                int RightDownTerrainType = MapData.TerrainTypeMap[(z - 1) * MapData.width + (x + 1)];
                AddSquare(Vertices, Uv, Tris, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding,
                    RightDownCenter + HexMetrics.corners[0] * HexSizeWithPadding,
                    RightDownCenter + HexMetrics.corners[5] * HexSizeWithPadding,
                    ThisCenter + HexMetrics.corners[3] * HexSizeWithPadding, thisTerrainType,
                    RightDownTerrainType, RightDownTerrainType, thisTerrainType
                );
            }

            #endregion

            #region init up right padding triangle

            if (x != MapData.width - 1)
            {
                if (z != MapData.height - 1)
                {
                    Vector3 RightUpCenter;
                    Color RightUpColor;
                    int RightUpTerrainType;
                    if (z % 2 == 0)
                    {
                        RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z + 1), MapData);
                        RightUpColor = MapData.ColorMap[(z + 1) * MapData.width + x];
                        RightUpTerrainType = MapData.TerrainTypeMap[(z + 1) * MapData.width + x];
                    }
                    else
                    {
                        RightUpCenter =
                            HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z + 1), MapData);
                        RightUpColor = MapData.ColorMap[(z + 1) * MapData.width + x + 1];
                        RightUpTerrainType = MapData.TerrainTypeMap[(z + 1) * MapData.width + x + 1];
                    }

                    AddTriangle(Vertices, Uv, Tris, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding,
                        RightUpCenter + HexMetrics.corners[3] * HexSizeWithPadding,
                        RightCenter + HexMetrics.corners[5] * HexSizeWithPadding,   
                        thisTerrainType, RightUpTerrainType, RightTerrainType);
                }
            }

            #endregion

            #region init down right padding triangle

            if (x != MapData.width - 1)
            {
                if (z != 0)
                {
                    Vector3 RightDownCenter;
                    Color RightDownColor;
                    int RightDownTerrainType;
                    if (z % 2 == 0)
                    {
                        RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z - 1), MapData);
                        RightDownColor = MapData.ColorMap[(z - 1) * MapData.width + x];
                        RightDownTerrainType = MapData.TerrainTypeMap[(z - 1) * MapData.width + x];
                    }
                    else
                    {
                        RightDownCenter =
                            HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z - 1), MapData);
                        RightDownColor = MapData.ColorMap[(z - 1) * MapData.width + x + 1];
                        RightDownTerrainType = MapData.TerrainTypeMap[(z - 1) * MapData.width + x + 1];
                    }

                    AddTriangle(Vertices, Uv, Tris,  ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding,
                        RightCenter + HexMetrics.corners[4] * HexSizeWithPadding,
                        RightDownCenter + HexMetrics.corners[0] * HexSizeWithPadding,thisTerrainType, RightTerrainType, RightDownTerrainType);
                }
            }

            #endregion

        }


        void AddSquare(List<Vector3> Vertices, List<Vector3> Uv, List<int> Tris, Vector3 v1,
            Vector3 v2, Vector3 v3, Vector3 v4,
            int terrainType1, int terrainType2, int terrainType3, int terrainType4)
        {
            AddTriangle(Vertices, Uv, Tris, v1, v2, v3, terrainType1, terrainType2, terrainType3);
            AddTriangle(Vertices, Uv, Tris, v3, v4, v1, terrainType3, terrainType4, terrainType1);
        }

        void AddTriangle(List<Vector3> OutVertices, List<Vector3> Uv, List<int> Tris, Vector3 v1, Vector3 v2,
            Vector3 v3, int terrainType1, int terrainType2, int terrainType3)
        {
            int VertCount = OutVertices.Count;
            OutVertices.Add(v1);
            OutVertices.Add(v2);
            OutVertices.Add(v3);

            Uv.Add(new Vector3(terrainType1, terrainType1, terrainType1));
            Uv.Add(new Vector3(terrainType2, terrainType2, terrainType2));
            Uv.Add(new Vector3(terrainType3, terrainType3, terrainType3));
            
            Tris.Add(VertCount);
            Tris.Add(VertCount + 1);
            Tris.Add(VertCount + 2);
        }

        void AddTriangle( List<Vector3> Vertices, List<Vector3> Uv,  List<int> Tris, Vector3 v1, Vector3 v2, Vector3 v3, int terrainType)
        {
            int VertCount = Vertices.Count;
            Vertices.Add(v1);
            Vertices.Add(v2);
            Vertices.Add(v3);

            Uv.Add(new Vector3(terrainType,terrainType,terrainType));
            Uv.Add(new Vector3(terrainType,terrainType,terrainType));
            Uv.Add(new Vector3(terrainType,terrainType,terrainType));

            Tris.Add(VertCount);
            Tris.Add(VertCount + 1);
            Tris.Add(VertCount + 2);
        }
    }
}
