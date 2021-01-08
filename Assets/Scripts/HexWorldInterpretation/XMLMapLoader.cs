using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

namespace HexWorldInterpretation
{
    static public class XMLMapLoader
    {
        static CultureInfo CInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
             

        static public List<HexGridData> MapLoadXMLFile(string path)
        {
            CInfo.NumberFormat.NumberDecimalSeparator = ".";
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
            float cellSize = float.Parse(MapNode.SelectSingleNode("cellSize").InnerText, CInfo);
            float cellPadding = float.Parse(MapNode.SelectSingleNode("cellPadding").InnerText, CInfo);
            float accuracyOfApproximation = float.Parse(MapNode.SelectSingleNode("accuracyOfApproximation").InnerText, CInfo);

            #region Only for test
            string MaterialPath = MapNode.SelectSingleNode("materialPath").InnerText;
            Material Default = Resources.Load<Material>(MaterialPath);
            #endregion

            float[] heightMap = new float[width * height];;
            Color[] colorMap = new Color[width * height];
            if (MapNode.SelectSingleNode("CellsData") == null)
            {
                heightMap = new float[width * height];
            }
            else
            {
                LoadCellsData(MapNode.SelectSingleNode("CellsData"), width, height, ref heightMap, ref colorMap);
            }
            return new HexGridData(name, width, height, cellSize, cellPadding, heightMap, colorMap,accuracyOfApproximation, Default,MaterialPath);
        }

        static void LoadCellsData(XmlNode HeightMapXMLNode, int width, int height, ref float[] heightMap, ref Color[] colorMap)
        {
            switch (HeightMapXMLNode.Attributes.GetNamedItem("type").Value)
            {
                case "overrideSave":
                    LoadCellsData_override(HeightMapXMLNode, width, height,ref heightMap,ref colorMap);
                    break;
            }
        }

        static void LoadCellsData_override(XmlNode HeightMap, int width, int height,ref float[] heightMap, ref Color[] colorMap)
        {
            foreach (XmlNode HexCellData in HeightMap.SelectNodes("Cell"))
            {
                int Xindex = 0;
                float Y = 0;
                int Zindex = 0;
                int.TryParse(HexCellData.SelectSingleNode("Xindex").InnerText, out Xindex);
                float.TryParse(HexCellData.SelectSingleNode("Y").InnerText, out Y);
                int.TryParse(HexCellData.SelectSingleNode("Zindex").InnerText, out Zindex);
                heightMap[Zindex * width + Xindex] = Y;

                XmlNode colorNode = HexCellData.SelectSingleNode("Color");

                if (colorNode != null)
                {
                    float r = float.Parse(colorNode.SelectSingleNode("Red").InnerText, CInfo);
                    float g = float.Parse(colorNode.SelectSingleNode("Green").InnerText, CInfo);
                    float b = float.Parse(colorNode.SelectSingleNode("Blue").InnerText, CInfo);

                    colorMap[Zindex * width + Xindex] = new Color(r,g,b);
                }
                else
                {
                    colorMap[Zindex * width + Xindex] = new Color();
                }
            }
        }

        static void LoadHeightMap_default(ref float[] heightMap, XmlNode HeightMap, int width, int height)
        {
            int z = 0;
            foreach (XmlNode HexRowData in HeightMap.SelectNodes("Row"))
            {
                char[] Separators = { '/', '#' };
                string[] CellData_String = HexRowData.InnerText.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < CellData_String.Length; x++)
                {
                    float.TryParse(CellData_String[x], NumberStyles.Any,CInfo, out heightMap[z * width + x]);
                }
                z++;
            }
        }
    }
}
