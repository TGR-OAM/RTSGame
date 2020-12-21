using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexWorldinterpretation
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
            Mesh ColumnMesh = new Mesh();

            List<Vector3> Vertices = new List<Vector3>();
            List<int> Tris = new List<int>();
            List<Color> colors = new List<Color>();

            float HexSizeWithPadding = MapData.cellSize - MapData.cellPadding;

            for (int z = 0; z < MapData.height; z++)
            {
                InitCell(Vertices, Tris, colors,x, z, HexSizeWithPadding);
            }

            ColumnMesh.vertices = Vertices.ToArray();
            ColumnMesh.triangles = Tris.ToArray();
            ColumnMesh.colors = colors.ToArray();

            ColumnMesh.RecalculateNormals();
            ColumnMesh.Optimize();

            Column.GetComponent<MeshFilter>().mesh = ColumnMesh;
            Column.GetComponent<MeshRenderer>().material = MapData.Default;
        }


        void InitCell(List<Vector3> Vertices,List<int> Tris, List<Color> colors,int x, int z,float HexSizeWithPadding)
        {
            Vector3 ThisCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z), MapData);

            Color thisColor = MapData.ColorMap[z * MapData.width + x];

            #region init hexagon inside

            for (int i = 2;i< HexMetrics.corners.Count();i++)
            {
                AddTriangle( Vertices,  Tris, colors, HexMetrics.corners[0] * HexSizeWithPadding + ThisCenter, HexMetrics.corners[i-1]* HexSizeWithPadding + ThisCenter, HexMetrics.corners[i]* HexSizeWithPadding + ThisCenter, thisColor);
            }

            #endregion

            #region init up in one column padding

            if(z != MapData.height-1)
            {
                Vector3 OtherCenterInThisColumn = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z + 1), MapData);

                Color OtherCenterInThisColumnColor = MapData.ColorMap[(z+1)*MapData.width+x];

                if (z % 2 == 0)
                {

                    AddSquare( Vertices,  Tris,  colors, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding, thisColor, OtherCenterInThisColumn + HexMetrics.corners[4] * HexSizeWithPadding, OtherCenterInThisColumnColor, OtherCenterInThisColumn + HexMetrics.corners[3] * HexSizeWithPadding, OtherCenterInThisColumnColor, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding, thisColor);
                }
                else
                {
                    AddSquare( Vertices,  Tris,  colors, ThisCenter + HexMetrics.corners[5] * HexSizeWithPadding, thisColor,OtherCenterInThisColumn + HexMetrics.corners[3] * HexSizeWithPadding, OtherCenterInThisColumnColor, OtherCenterInThisColumn + HexMetrics.corners[2] * HexSizeWithPadding, OtherCenterInThisColumnColor, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding, thisColor);
                }
            }

            #endregion

            #region init right padding

            Vector3 RightCenter = new Vector3();
            Color RightColor = new Color();

            if (x != MapData.width - 1)
            {
                RightCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z), MapData);
                RightColor = MapData.ColorMap[z * MapData.width + (x + 1)];
                AddSquare( Vertices,  Tris,  colors, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding, thisColor,RightCenter + HexMetrics.corners[5] * HexSizeWithPadding, RightColor, RightCenter + HexMetrics.corners[4] * HexSizeWithPadding, RightColor, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding,thisColor);
            }

            #endregion

            #region init right up padding

            if (z % 2 == 1 && z != MapData.height - 1 && x != MapData.width - 1)
            {
                Vector3 RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z + 1), MapData);
                Color RightUpColor = MapData.ColorMap[(z+1) * MapData.width + (x + 1)];
                AddSquare( Vertices,  Tris,  colors, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding, thisColor,RightUpCenter + HexMetrics.corners[4] * HexSizeWithPadding, RightUpColor,RightUpCenter + HexMetrics.corners[3] * HexSizeWithPadding,RightUpColor, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding,thisColor);
            }

            #endregion

            #region init right down padding

            if (z % 2 == 1 && z != 0 && x != MapData.width-1)
            {
                Vector3 RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x+1, 0, z - 1), MapData);
                Color RightDownColor = MapData.ColorMap[(z - 1) * MapData.width + (x + 1)];
                AddSquare( Vertices,  Tris,  colors, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding, thisColor,RightDownCenter + HexMetrics.corners[0] * HexSizeWithPadding, RightDownColor, RightDownCenter + HexMetrics.corners[5] * HexSizeWithPadding, RightDownColor, ThisCenter + HexMetrics.corners[3] * HexSizeWithPadding, thisColor);
            }

            #endregion

            #region init up right padding triangle

            if (x != MapData.width - 1)
            {
                if (z != MapData.height - 1)
                {
                    Vector3 RightUpCenter;
                    Color RightUpColor;
                    if (z % 2 == 0)
                    {
                        RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z + 1), MapData);
                        RightUpColor = MapData.ColorMap[(z+1)*MapData.width+x];
                    }
                    else
                    {
                        RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z + 1), MapData);
                        RightUpColor = MapData.ColorMap[(z + 1) * MapData.width + x+1];
                    }
                    AddTriangle( Vertices,  Tris,  colors, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding, RightUpCenter + HexMetrics.corners[3] * HexSizeWithPadding, RightCenter + HexMetrics.corners[5] * HexSizeWithPadding,thisColor,RightUpColor,RightColor);
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
                    if (z % 2 == 0)
                    {
                        RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x, 0, z - 1), MapData);
                        RightDownColor = MapData.ColorMap[(z - 1) * MapData.width + x];
                    }
                    else
                    {
                        RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(x + 1, 0, z - 1), MapData);
                        RightDownColor = MapData.ColorMap[(z - 1) * MapData.width + x+1];
                    }
                    AddTriangle( Vertices, Tris, colors, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding, RightCenter + HexMetrics.corners[4] * HexSizeWithPadding, RightDownCenter + HexMetrics.corners[0] * HexSizeWithPadding, thisColor,  RightColor, RightDownColor);

                }
            }

            #endregion

        }


        void AddSquare( List<Vector3> Vertices,  List<int> Tris,  List<Color> colors, Vector3 v1, Color color1, Vector3 v2, Color color2, Vector3 v3, Color color3, Vector3 v4, Color color4)
        {
            AddTriangle( Vertices,  Tris,  colors, v1,  v2,  v3, color1, color2, color3);
            AddTriangle( Vertices,  Tris,  colors, v3, v4,  v1, color3, color4, color1);
        }

        void AddTriangle( List<Vector3> OutVertices,  List<int> Tris,  List<Color> colors, Vector3 v1, Vector3 v2, Vector3 v3, Color color1, Color color2, Color color3)
        {
            int VertCount = OutVertices.Count;
            OutVertices.Add(v1);
            OutVertices.Add(v2);
            OutVertices.Add(v3);

            colors.Add(color1);
            colors.Add(color2);
            colors.Add(color3);

            Tris.Add(VertCount);
            Tris.Add(VertCount + 1);
            Tris.Add(VertCount + 2);
        }

        void AddTriangle( List<Vector3> Vertices,  List<int> Tris,  List<Color> colors, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
        {
            int VertCount = Vertices.Count;
            Vertices.Add(v1);
            Vertices.Add(v2);
            Vertices.Add(v3);

            colors.Add(color);
            colors.Add(color);
            colors.Add(color);

            Tris.Add(VertCount);
            Tris.Add(VertCount + 1);
            Tris.Add(VertCount + 2);
        }
    }
}
