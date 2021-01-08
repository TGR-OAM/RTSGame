using System.Xml.Linq;
using UnityEngine;
using UnityEditor;
using System.Globalization;
using HexWorldInterpretation;

namespace HexWordEditor
{    
    public enum SaveType
    { 
        defaultSave,
        overrideSave,
    }


    static public class XMLMapSaver
    {
        static CultureInfo CInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();

        static public void MapSaverXMLFile(HexGridData MapToSave, string path,SaveType saveType)
        {

            CInfo.NumberFormat.NumberDecimalSeparator = ".";

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

            XElement CellsData = new XElement("CellsData", new XAttribute("type", saveType.ToString()));
            

            XElement ColorMap = new XElement("ColorMap", new XAttribute("type", SaveType.overrideSave));

            switch (saveType)
            {
                case SaveType.overrideSave:
                    MapSaverXMLFile_OverrideSave(ref CellsData, ref ColorMap, MapToSave);
                    break;

            }
            MapData.Add(CellsData);
            MapData.Add(ColorMap);

            XmlDoc.Save(Application.dataPath+"/"+ "Resources/"+ path + ".xml");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static void MapSaverXMLFile_DefaultSave(ref XElement HeightMap, ref XElement ColorMap, HexGridData MapData)
        {
            for (int z = 0; z < MapData.height; z++)
            {
                string rowValue = "";
                for (int x = 0; x < MapData.width; x++)
                {
                    rowValue += MapData.HeightMap[z * MapData.width + x].ToString(CInfo) + "/";

                    XElement Cell = new XElement("Cell");

                    Cell.Add(new XElement("Xindex", x));
                    Cell.Add(new XElement("Zindex", z));

                    XElement Color = new XElement("Color");

                    Color.Add(new XElement("Red", MapData.ColorMap[z * MapData.width + x].r));
                    Color.Add(new XElement("Green", MapData.ColorMap[z * MapData.width + x].g));
                    Color.Add(new XElement("Blue", MapData.ColorMap[z * MapData.width + x].b));

                    Cell.Add(Color);
                    ColorMap.Add(Cell);
                }
                HeightMap.Add(new XElement("Row", rowValue));
            }
        }

        static void MapSaverXMLFile_OverrideSave(ref XElement HeightMap, ref XElement ColorMap, HexGridData MapData)
        {
            for (int z = 0; z < MapData.height; z++)
            {
                for (int x = 0; x < MapData.width; x++)
                {
                    #region SaveCell
                    if(MapData.HeightMap[z * MapData.width + x] != 0 || MapData.ColorMap[z * MapData.width + x] != MapData.Default.color)
                    {
                        XElement Cell = new XElement("Cell");

                        Cell.Add(new XElement("Xindex", x));
                        Cell.Add(new XElement("Zindex", z));
                        Cell.Add(new XElement("Y", MapData.HeightMap[z * MapData.width + x].ToString(CInfo)));

                        XElement Color = new XElement("Color");

                        Color.Add(new XElement("Red", MapData.ColorMap[z * MapData.width + x].r));
                        Color.Add(new XElement("Green", MapData.ColorMap[z * MapData.width + x].g));
                        Color.Add(new XElement("Blue", MapData.ColorMap[z * MapData.width + x].b));

                        Cell.Add(Color);
                        
                        HeightMap.Add(Cell);
                    }
                    #endregion
                }
            }
        }
    }
}
