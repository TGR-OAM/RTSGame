using System.Collections.Generic;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.HexWorldinterpretation
{
    public class HexGrid : MonoBehaviour
    {
        public string DefMapPath;

        public HexGridData MapData;

        public HexGridRenderer hexGridRenderer { get; private set; }
        public HexGridColiderer hexGridColiderer { get; private set; }

        private void Awake()
        {
            MapData = XMLMapLoader.MapLoadXMLFile(DefMapPath)[0];

            InitWorld();
        }


        void InitWorld()
        {
            #region init mesh
            hexGridRenderer = new HexGridRenderer(this);
            #endregion

            #region init coliders
            hexGridColiderer = new HexGridColiderer(this,9);
            #endregion
        }

    

        public bool TryRaycastHexGrid( Ray rayToCast, out Vector3 output)
        {
            return hexGridColiderer.TryRaycastHexGrid( rayToCast, out output);
        }

    }

    [System.Serializable]
    public struct HexGridData
    {
        #region this gets with resource load

        public string name { get; private set; }
        public float AccurcyOfApproximation { get; private set; }
     
        public int width { get; private set; }
        public int height { get; private set; }

        public float widthInUnits { get; private set; }
        public float heightInUnits { get; private set; }

        public float cellSize { get; private set; }
        public float cellPadding { get; private set; }

        public float[] HeightMap;

        public Color[] ColorMap;

        #endregion

        #region this isnt permanent solution
        public string materialPath;
        public Material Default;
        #endregion

        #region other information about map
        public List<Building> ConstructedBuildings;
        #endregion


        public HexGridData(string name,int width,int height, float cellSize, float padding, float AccurcyOfApproximation, Material Default,string materialPath)
        {
            this.name = name;

            this.width = width;
            this.height = height;

            this.cellSize = cellSize;
            this.cellPadding = padding;

            this.HeightMap = new float[width* height];

            this.ColorMap = new Color[width * height];
            for(int i =0;i< width * height;i++)
            {
                ColorMap[i] = Default.color;
            }

            this.Default = Default;
            this.materialPath = materialPath;

            this.AccurcyOfApproximation = AccurcyOfApproximation;

            this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
            this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

            ConstructedBuildings = new List<Building>();

        }
        public HexGridData(string name, int width, int height, float cellSize, float padding, float[] HeightMap, float AccurcyOfApproximation, Material Default,string materialPath)
        {
            this.name = name;


            this.width = width;
            this.height = height;

            this.cellSize = cellSize;
            this.cellPadding = padding;

            this.HeightMap = HeightMap;

            this.ColorMap = new Color[width * height];
            for (int i = 0; i < width * height; i++)
            {
                ColorMap[i] = Default.color;
            }

            this.Default = Default;
            this.materialPath = materialPath;

            this.AccurcyOfApproximation = AccurcyOfApproximation;

            this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
            this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

            ConstructedBuildings = new List<Building>();

        }
        public HexGridData(string name, int width, int height, float cellSize, float padding, float[] HeightMap, Color[] ColorMap,float AccurcyOfApproximation, Material Default, string materialPath)
        {
            this.name = name;


            this.width = width;
            this.height = height;

            this.cellSize = cellSize;
            this.cellPadding = padding;

            this.HeightMap = HeightMap;

            this.ColorMap = ColorMap;

            this.Default = Default;
            this.materialPath = materialPath;

            this.AccurcyOfApproximation = AccurcyOfApproximation;

            this.widthInUnits = width * HexMetrics.innerRadius * 2f * cellSize + (height > 1f ? HexMetrics.innerRadius * cellSize : 0);
            this.heightInUnits = 1.5f * (height - 1) * cellSize * HexMetrics.outerRadius + cellSize * HexMetrics.outerRadius * 2f;

            ConstructedBuildings = new List<Building>();

        }
    }
}