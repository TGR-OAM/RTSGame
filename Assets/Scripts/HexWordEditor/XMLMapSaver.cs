using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.HexWordEditor
{
    public enum SaveType
    { 
        defaultSave,
        overrideSave,
    }


    static public class XMLMapSaver
    {
        static public void MapSaverXMLFile(HexGridData MapToSave, string path, SaveType saveType)
        {
            XDocument XmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("MapDatas"));

            XElement MapData = new XElement("MapData", new XAttribute("name", MapToSave.name));
            XmlDoc.Root.Add(MapData);

            MapData.Add(new XElement("width", MapToSave.width));
            MapData.Add(new XElement("height", MapToSave.height));

            MapData.Add(new XElement("cellSize", MapToSave.cellSize));
            MapData.Add(new XElement("cellPadding", MapToSave.cellPadding));

            MapData.Add(new XElement("accuracyOfApproximation", MapToSave.AccurcyOfApproximation));

            MapData.Add(new XElement("materialPath", MapToSave.materialPath));

            XElement HeightMap = new XElement("HeightMap", new XAttribute("type", saveType.ToString()));
            

            XElement ColorMap = new XElement("ColorMap", new XAttribute("type", SaveType.overrideSave));

            switch (saveType)
            {
                case SaveType.defaultSave:
                    MapSaverXMLFile_DefaultSave(ref HeightMap,ref ColorMap, MapToSave);
                    break;

                case SaveType.overrideSave:

                    break;

            }
            MapData.Add(HeightMap);
            MapData.Add(ColorMap);

            XmlDoc.Save(Application.dataPath + "/"+MapToSave.name + ".xml");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static void MapSaverXMLFile_DefaultSave(ref XElement HeightMap,ref XElement ColorMap,HexGridData MapData)
        {
            for (int z = 0; z < MapData.height; z++)
            {
                string rowValue = "";
                for (int x = 0; x < MapData.width; x++)
                {
                    rowValue += MapData.HeightMap[z * MapData.width + x].ToString() + "/";

                    XElement Cell = new XElement("Cell");

                    Cell.Add(new XElement("Xindex", x));
                    Cell.Add(new XElement("Zindex", z));

                    XElement Color = new XElement("Color");

                    Color.Add("Red", MapData.ColorMap[z * MapData.width + x].r);
                    Color.Add("Green", MapData.ColorMap[z * MapData.width + x].g);
                    Color.Add("Blue", MapData.ColorMap[z * MapData.width + x].b);

                    Cell.Add(Color);
                    ColorMap.Add(Cell);
                }
                HeightMap.Add(new XElement("Row",rowValue));
            }
        }
    }
}
