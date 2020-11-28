using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.HexWorldinterpretation
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

            float[] heightMap;
            if (MapNode.SelectSingleNode("HeightMap") == null)
            {
                heightMap = new float[width * height];
            }
            else
            {
                heightMap = LoadHeightMap(MapNode.SelectSingleNode("HeightMap"), width, height);
            }
            if(MapNode.SelectSingleNode("ColorMap") == null)
            {
                return new HexGridData(name, width, height, cellSize, cellPadding, heightMap, accuracyOfApproximation, Default,MaterialPath);
            }

            Color[] colorMap = LoadColorMap(MapNode.SelectSingleNode("ColorMap"), width, height, Default.color);

            return new HexGridData(name, width, height, cellSize, cellPadding, heightMap, colorMap, accuracyOfApproximation, Default, MaterialPath);

        }

        static float[] LoadHeightMap(XmlNode HeightMapXMLNode, int width, int height)
        {
            float[] heightMap = new float[width * height];

            switch (HeightMapXMLNode.Attributes.GetNamedItem("type").Value)
            {
                case "overrideSave":
                    LoadHeightMap_override(ref heightMap, HeightMapXMLNode, width, height);
                    break;

                case "defaultSave":
                    LoadHeightMap_default(ref heightMap, HeightMapXMLNode, width, height);
                    break;
            }
            return heightMap;
        }

        static void LoadHeightMap_override(ref float[] heightMap, XmlNode HeightMap, int width, int height)
        {
            foreach (XmlNode HexCellData in HeightMap.SelectNodes("Cell"))
            {
                int Xindex = int.Parse(HexCellData.SelectSingleNode("Xindex").InnerText);
                float Y = float.Parse(HexCellData.SelectSingleNode("Y").InnerText, CInfo);
                int Zindex = int.Parse(HexCellData.SelectSingleNode("Zindex").InnerText);
                heightMap[Zindex * width + Xindex] = Y;
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
                    heightMap[z * width + x] = float.Parse(CellData_String[x], CInfo);
                }
                z++;
            }
        }

        static Color[] LoadColorMap(XmlNode HeightMapXMLNode, int width, int height, Color defaultColor)
        {
            Color[] colorMap = new Color[width * height];

            switch (HeightMapXMLNode.Attributes.GetNamedItem("type").Value)
            {
                case "overrideSave":
                    LoadColorMap_override(ref colorMap, HeightMapXMLNode, width, height, defaultColor);
                    break;
            }
            return colorMap;
        }


        static void LoadColorMap_override(ref Color[] colorMap, XmlNode ColorMap, int width, int height, Color defaultColor)
        {
            for(int i =0;i<colorMap.Length;i++)
            {
                colorMap[i] = defaultColor;
            }

            foreach (XmlNode HexCellData in ColorMap.SelectNodes("Cell"))
            {
                int Xindex = int.Parse(HexCellData.SelectSingleNode("Xindex").InnerText);
                int Zindex = int.Parse(HexCellData.SelectSingleNode("Zindex").InnerText);

                XmlNode colorNode = HexCellData.SelectSingleNode("Color");

                float r = float.Parse(colorNode.SelectSingleNode("Red").InnerText, CInfo);
                float g = float.Parse(colorNode.SelectSingleNode("Green").InnerText, CInfo);
                float b = float.Parse(colorNode.SelectSingleNode("Blue").InnerText, CInfo);

                colorMap[Zindex * width + Xindex] = new Color(r,g,b);
            }
        }
    }
}
