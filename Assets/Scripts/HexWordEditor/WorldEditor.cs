using System;
using System.Collections.Generic;
using Assets.Scripts.HexWorldinterpretation;
using UnityEngine;

namespace Assets.Scripts.HexWordEditor
{
    public class WorldEditor
    {
        HexGridRenderer hexGridRenderer;
        HexGridColiderer hexGridColiderer;
        HexGridData MapData;

        public WorldEditor(HexGrid hexGrid)
        {
            this.hexGridColiderer = hexGrid.hexGridColiderer;
            this.hexGridRenderer = hexGrid.hexGridRenderer;

            this.MapData = hexGrid.MapData;
        }

        public void TryUpdateCellHeight(List<Vector3> Coords, float dY)
        {
            for (int i = 0; i < Coords.Count; i++)
            {
                if (Coords[i].x < 0 || Coords[i].z < 0 || Coords[i].x > MapData.width - 1 || Coords[i].z > MapData.height - 1)
                {
                    Coords.RemoveAt(i);
                    i--;
                    continue;
                }
                else
                    MapData.HeightMap[(int)Coords[i].z * MapData.width + (int)Coords[i].x] += dY;
            }
            hexGridRenderer.UpdateHexGridMesh(Coords);
            hexGridColiderer.ReUpdateAllColiders();
        }
        public void TryUpdateCellHeight(List<Vector3> Coords, float dY,float targetHeight)
        {
            for (int i = 0; i < Coords.Count; i++)
            {
                if (Coords[i].x < 0 || Coords[i].z < 0 || Coords[i].x > MapData.width - 1 || Coords[i].z > MapData.height - 1)
                {
                    Coords.RemoveAt(i);
                    i--;
                    continue;
                }
                else
                {
                    float CurHeight = MapData.HeightMap[(int)Coords[i].z * MapData.width + (int)Coords[i].x];
                    if (CurHeight < targetHeight)
                    {
                        MapData.HeightMap[(int)Coords[i].z * MapData.width + (int)Coords[i].x] = Math.Min(CurHeight + dY, targetHeight);
                    }
                    else if (CurHeight > targetHeight)
                    {
                        MapData.HeightMap[(int)Coords[i].z * MapData.width + (int)Coords[i].x] = Math.Max(CurHeight - dY, targetHeight);
                    }
                    else continue;
                }
            }
            hexGridRenderer.UpdateHexGridMesh(Coords);
            hexGridColiderer.ReUpdateAllColiders();
        }
        public void TryUpdateCellHeight(Vector3 Coord, float dY)
        {
            if (Coord.x < 0 || Coord.z < 0)
            {
                return;
            }
            MapData.HeightMap[(int)Coord.z * MapData.width + (int)Coord.x] += dY;
            List<Vector3> Coords = new List<Vector3>();
            Coords.Add(Coord);
            hexGridRenderer.UpdateHexGridMesh(Coords);
            hexGridColiderer.ReUpdateAllColiders();
        }
        public void TryUpdateCellHeight(Vector3 Coord, float dY, float targetHeight)
        {
            if (Coord.x < 0 || Coord.z < 0)
            {
                return;
            }
            float CurHeight = MapData.HeightMap[(int)Coord.z * MapData.width + (int)Coord.x];
            if (CurHeight < targetHeight)
            {
                MapData.HeightMap[(int)Coord.z * MapData.width + (int)Coord.x] = Math.Min(CurHeight + dY, targetHeight);
            }
            else if (CurHeight > targetHeight)
            {
                MapData.HeightMap[(int)Coord.z * MapData.width + (int)Coord.x] = Math.Max(CurHeight - dY, targetHeight);
            }
            else return;
            List<Vector3> Coords = new List<Vector3>();
            Coords.Add(Coord);
            hexGridRenderer.UpdateHexGridMesh(Coords);
            hexGridColiderer.ReUpdateAllColiders();
        }



        public void TryUpdateCellHeightInRadius(Vector3 Center, float dY, int radius)
        {
            List<Vector3> AllCenters = new List<Vector3>();

            if (Center.z % 2 == 0)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    int x_max = radius * 2 - Mathf.Abs(z);
                    if (x_max % 2 == 0)
                        for (int x = -x_max / 2; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x, 0, z) + Center);
                        }
                    else
                        for (int x = -x_max / 2 - 1; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x, 0, z) + Center);
                        }
                }
            }
            else
            {
                for (int z = -radius; z <= radius; z++)
                {
                    int x_max = radius * 2 - Mathf.Abs(z);
                    if (x_max % 2 == 0)
                        for (int x = -x_max / 2; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x - 1, 0, z - 1) + Center);
                        }
                    else
                        for (int x = -x_max / 2 - 1; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x - 1, 0, z - 1) + Center);
                        }
                }
            }

            TryUpdateCellHeight(AllCenters, dY);
        }

        public void TryUpdateCellHeightInRadius(Vector3 Center, float dY, int radius, float targetHeight)
        {
            List<Vector3> AllCenters = new List<Vector3>();

            if (Center.z % 2 == 0)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    int x_max = radius * 2 - Mathf.Abs(z);
                    if (x_max % 2 == 0)
                        for (int x = -x_max / 2; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x, 0, z) + Center);
                        }
                    else
                        for (int x = -x_max / 2 - 1; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x, 0, z) + Center);
                        }
                }
            }
            else
            {
                for (int z = -radius; z <= radius; z++)
                {
                    int x_max = radius * 2 - Mathf.Abs(z);
                    if (x_max % 2 == 0)
                        for (int x = -x_max / 2; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x - 1, 0, z - 1) + Center);
                        }
                    else
                        for (int x = -x_max / 2 - 1; x <= x_max / 2; x++)
                        {
                            AllCenters.Add(new Vector3(x - 1, 0, z - 1) + Center);
                        }
                }
            }

            TryUpdateCellHeight(AllCenters, dY, targetHeight);
        }


        public void TryUpdateCellColor(List<Vector3> Coords, Color color)
        {
            for (int i = 0; i < Coords.Count; i++)
            {
                if (Coords[i].x < 0 || Coords[i].z < 0 || Coords[i].x > MapData.width - 1 || Coords[i].z > MapData.height - 1)
                {
                    Coords.RemoveAt(i);
                    i--;
                    continue;
                }
                else
                    MapData.ColorMap[(int)Coords[i].z * MapData.width + (int)Coords[i].x] = color;
            }
            hexGridRenderer.UpdateHexGridMesh(Coords);
            hexGridColiderer.ReUpdateAllColiders();
        }

        public void TryUpdateCellColor(Vector3 Coord, Color color)
        {
            if (Coord.x < 0 || Coord.z < 0 || Coord.x > MapData.width - 1 || Coord.z > MapData.height - 1)
            {
                return;
            }
            else
            {
                MapData.ColorMap[(int)Coord.z * MapData.width + (int)Coord.x] = color;
            }

            List<Vector3> Coords = new List<Vector3>();
            Coords.Add(Coord);
            hexGridRenderer.UpdateHexGridMesh(Coords);
            hexGridColiderer.ReUpdateAllColiders();
        }
    }
}
