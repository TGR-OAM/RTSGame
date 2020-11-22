using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HexColumnRenderer
    {
        HexGridData UploadedData;
        GameObject Column;

        public HexColumnRenderer(int x,HexGrid hexGrid, Transform MeshPart)
        {
            this.UploadedData = hexGrid.MapData;

            Column = new GameObject("Column " + x, typeof(MeshRenderer),typeof(MeshFilter));
            Column.transform.localPosition = new Vector3(HexMetrics.innerRadius*2*UploadedData.cellSize*x,0,0);
            Column.transform.parent = MeshPart;

            UpdateColumn(x);
        }

        public void UpdateColumn(int x)
        {
            Mesh ColumnMesh = new Mesh();

            List<Vector3> Vertices = new List<Vector3>();
            List<int> Tris = new List<int>();

            float HexSizeWithPadding = UploadedData.cellSize - UploadedData.padding;

            for (int z = 0; z < UploadedData.height; z++)
            {
                InitCell(ref Vertices, ref Tris, x, z, HexSizeWithPadding);
            }

            ColumnMesh.vertices = Vertices.ToArray();
            ColumnMesh.triangles = Tris.ToArray();

            ColumnMesh.RecalculateNormals();
            ColumnMesh.Optimize();

            Column.GetComponent<MeshFilter>().mesh = ColumnMesh;
            Column.GetComponent<MeshRenderer>().material = UploadedData.Default;
        }


        void InitCell(ref List<Vector3> Vertices,ref List<int> Tris,int x, int z,float HexSizeWithPadding)
        {
            Vector3 ThisCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(0,0,z),UploadedData) + GetHeightByHexCoord(x,z);

            #region init hexagon inside

            for (int i = 2;i< HexMetrics.corners.Count();i++)
            {
                AddTriangle(ref Vertices, ref Tris, HexMetrics.corners[0] * HexSizeWithPadding + ThisCenter, HexMetrics.corners[i-1]* HexSizeWithPadding + ThisCenter, HexMetrics.corners[i]* HexSizeWithPadding + ThisCenter);
            }

            #endregion

            #region init up in one column padding

            if(z != UploadedData.height-1)
            {
                Vector3 OtherCenterInThisColumn = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(0, 0, z + 1), UploadedData)+ GetHeightByHexCoord(x, z+1);

                if (z % 2 == 0)
                {
                    AddSquare(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding, OtherCenterInThisColumn + HexMetrics.corners[4] * HexSizeWithPadding, OtherCenterInThisColumn + HexMetrics.corners[3] * HexSizeWithPadding, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding);
                }
                else
                {
                    AddSquare(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[5] * HexSizeWithPadding, OtherCenterInThisColumn + HexMetrics.corners[3] * HexSizeWithPadding, OtherCenterInThisColumn + HexMetrics.corners[2] * HexSizeWithPadding, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding);
                }
            }

            #endregion

            #region init right padding

            Vector3 RightCenter = new Vector3();

            if (x != UploadedData.width - 1)
            {
                RightCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(1, 0, z), UploadedData) + GetHeightByHexCoord(x+1, z);
                AddSquare(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding, RightCenter + HexMetrics.corners[5] * HexSizeWithPadding, RightCenter + HexMetrics.corners[4] * HexSizeWithPadding, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding);
            }

            #endregion

            #region init right up padding

            if (z % 2 == 1 && z != UploadedData.height - 1 && x != UploadedData.width - 1)
            {
                Vector3 RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(1, 0, z + 1), UploadedData) + GetHeightByHexCoord(x+1, z+1);

                AddSquare(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[0] * HexSizeWithPadding, RightUpCenter + HexMetrics.corners[4] * HexSizeWithPadding, RightUpCenter + HexMetrics.corners[3] * HexSizeWithPadding, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding);
            }

            #endregion

            #region init right down padding

            if (z % 2 == 1 && z != 0 && x != UploadedData.width-1)
            {
                Vector3 RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(1, 0, z - 1), UploadedData) + GetHeightByHexCoord(x+1, z-1);
                AddSquare(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding, RightDownCenter + HexMetrics.corners[0] * HexSizeWithPadding, RightDownCenter + HexMetrics.corners[5] * HexSizeWithPadding, ThisCenter + HexMetrics.corners[3] * HexSizeWithPadding);
            }

            #endregion

            #region init up right padding triangle

            if (x != UploadedData.width - 1)
            {
                if (z != UploadedData.height - 1)
                {
                    Vector3 RightUpCenter;
                    if (z % 2 == 0)
                    {
                        RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(0, 0, z + 1), UploadedData) + GetHeightByHexCoord(x, z + 1);
                       
                    }
                    else
                    {
                        RightUpCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(1, 0, z + 1), UploadedData) + GetHeightByHexCoord(x + 1, z + 1);
                        
                    }
                    AddTriangle(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[1] * HexSizeWithPadding, RightUpCenter + HexMetrics.corners[3] * HexSizeWithPadding, RightCenter + HexMetrics.corners[5] * HexSizeWithPadding);
                }
            }

            #endregion

            #region init down right padding triangle

            if (x != UploadedData.width - 1)
            {
                if (z != 0)
                {
                    Vector3 RightDownCenter;
                    if (z % 2 == 0)
                    {
                        RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(0, 0, z - 1), UploadedData) + GetHeightByHexCoord(x, z - 1);
                    }
                    else
                    {
                        RightDownCenter = HexMetrics.CalcCenterCoordXZFromHexCoordXZ(new Vector3(1, 0, z - 1), UploadedData) + GetHeightByHexCoord(x + 1, z - 1);
                    }
                    AddTriangle(ref Vertices, ref Tris, ThisCenter + HexMetrics.corners[2] * HexSizeWithPadding, RightCenter + HexMetrics.corners[4] * HexSizeWithPadding, RightDownCenter + HexMetrics.corners[0] * HexSizeWithPadding);

                }
            }

            #endregion

        }


        void AddSquare(ref List<Vector3> Vertices, ref List<int> Tris, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            AddTriangle(ref Vertices, ref Tris, v1, v2, v3);
            AddTriangle(ref Vertices, ref Tris, v3, v4, v1);
        }

        void AddTriangle(ref List<Vector3> Vertices, ref List<int> Tris, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int VertCount = Vertices.Count;
            Vertices.Add(v1);
            Vertices.Add(v2);
            Vertices.Add(v3);

            Tris.Add(VertCount);
            Tris.Add(VertCount + 1);
            Tris.Add(VertCount + 2);
        }


        Vector3 GetHeightByHexCoord(int x, int z)
        {
            return new Vector3(0,UploadedData.HeightMap[z*UploadedData.width+x],0);
        }
    }
}
