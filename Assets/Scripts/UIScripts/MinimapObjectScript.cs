using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapObjectScript:MonoBehaviour
{
    public UIManager Manager;

    public RawImage RenderMinimapTexture;

    HexGrid hexGrid;

    GameObject MinimapCamera;

    RenderTexture CameraRenderTexture;

    private void Start()
    {
        this.hexGrid = Manager.hexGrid;
        
        CameraRenderTexture = new RenderTexture((int)hexGrid.MapData.widthInUnits, (int)hexGrid.MapData.heightInUnits, 16,RenderTextureFormat.ARGB32);
        CameraRenderTexture.Create();

        CreateAndSetCamera();
    }

    void CreateAndSetCamera()
    {
        MinimapCamera = new GameObject("Minimap Camera", typeof(Camera));
        MinimapCamera.transform.parent = hexGrid.transform;


        Camera CameraComponent = MinimapCamera.GetComponent<Camera>();
        CameraComponent.orthographic = true;
        CameraComponent.nearClipPlane = 0;
        CameraComponent.farClipPlane = 100000f;

        CameraComponent.transform.position = new Vector3(hexGrid.MapData.widthInUnits / 2f - hexGrid.MapData.cellSize * HexMetrics.innerRadius, 10000, hexGrid.MapData.heightInUnits / 2f - hexGrid.MapData.cellSize * HexMetrics.outerRadius);
        CameraComponent.transform.rotation = Quaternion.Euler(90, 0, 0);
        CameraComponent.orthographicSize = Mathf.Min(hexGrid.MapData.widthInUnits, hexGrid.MapData.heightInUnits) / 2;

        //CameraComponent.rect = new Rect(0, 1 - Mathf.Min(hexGrid.MapData.widthInUnits, hexGrid.MapData.heightInUnits) / Mathf.Max(hexGrid.MapData.widthInUnits, hexGrid.MapData.heightInUnits), 1, 1);

        CameraComponent.targetTexture = CameraRenderTexture;
        RenderMinimapTexture.texture = CameraRenderTexture;
    }

}
