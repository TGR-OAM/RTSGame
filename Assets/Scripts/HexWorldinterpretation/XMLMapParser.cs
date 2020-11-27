using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.HexWorldinterpretation
{
    static public class XMLMapParser
    {
        static public List<HexGridData> LoadXMLFileMap(string path)
        {
            TextAsset XML_String = Resources.Load<TextAsset>(path);
            XmlDocument XmlData = new XmlDocument();
            XmlData.LoadXml(XML_String.text);
            XmlNode RootNode  = XmlData.SelectSingleNode("MapDatas");
            List<HexGridData> hexGridDatas = new List<HexGridData>();
            foreach (XmlNode MapNode in RootNode.SelectNodes("MapData"))
            {
                hexGridDatas.Add(LoadMap(MapNode));
            }

            return hexGridDatas;
        }

        static HexGridData LoadMap(XmlNode MapNode)
        {
            string name = MapNode.Attributes.GetNamedItem("name").Value;
            int width = int.Parse(MapNode.SelectSingleNode("width").InnerText);
            int height = int.Parse(MapNode.SelectSingleNode("height").InnerText);
            float cellSize = float.Parse(MapNode.SelectSingleNode("cellSize").InnerText);
            float cellPadding = float.Parse(MapNode.SelectSingleNode("cellPadding").InnerText);
            float accuracyOfApproximation = float.Parse(MapNode.SelectSingleNode("accuracyOfApproximation").InnerText);

            float[] heightMap = LoadHeightMap(MapNode.SelectSingleNode("HeightMap"), width, height);

            #region Only for test
            string MaterialPath = MapNode.SelectSingleNode("materialPath").InnerText;
            Material Default = Resources.Load<Material>(MaterialPath);
            #endregion

            return new HexGridData(name, width, height, cellSize, cellPadding, heightMap, accuracyOfApproximation, Default);
        }

        static float[] LoadHeightMap(XmlNode HeightMapXMLNode, int width, int height)
        {
            float[] heightMap = new float[width * height];

            switch (HeightMapXMLNode.Attributes.GetNamedItem("type").Value)
            {
                case "override":
                    LoadHeightMap_override(heightMap, HeightMapXMLNode, width, height);
                    break;

                case "default":
                    LoadHeightMap_default(heightMap, HeightMapXMLNode, width, height);
                    break;
            }
            return heightMap;
        }

        static float[] LoadHeightMap_override(float[] heightMap, XmlNode HeightMap, int width, int height)
        {
            foreach (XmlNode HexCellData in HeightMap.SelectNodes("Cell"))
            {
                int Xindex = int.Parse(HexCellData.SelectSingleNode("Xindex").InnerText);
                float Y = float.Parse(HexCellData.SelectSingleNode("Y").InnerText);
                int Zindex = int.Parse(HexCellData.SelectSingleNode("Zindex").InnerText);
                heightMap[Zindex * width + Xindex] = Y;
            }
            return heightMap;
        }

        static float[] LoadHeightMap_default(float[] heightMap, XmlNode HeightMap, int width, int height)
        {
            int z = 0;
            foreach (XmlNode HexRowData in HeightMap.SelectNodes("Row"))
            {
                char[] Separators = { '/', '#' };
                string[] CellData_String = HexRowData.InnerText.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < CellData_String.Length; x++)
                {
                    heightMap[z * width + x] = float.Parse(CellData_String[x]);
                }
                z++;
            }
            return heightMap;
        }
    }
}
