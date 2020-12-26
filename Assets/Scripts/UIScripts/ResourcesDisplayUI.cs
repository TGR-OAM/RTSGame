using System;
using System.Collections;
using System.Collections.Generic;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesDisplayUI : MonoBehaviour
{
    [SerializeField] private Text[] resourcesTexts; // 0 - neoSteel 1 - titanium 2 - unoBtanium

    public void UpdateResourcesValues(GameResourceStruct gameResourceStruct)
    {
        resourcesTexts[0].text = gameResourceStruct.neoSteel.ToString();
        resourcesTexts[1].text = gameResourceStruct.titanium.ToString();
        resourcesTexts[2].text = gameResourceStruct.unobtanium.ToString();
    }
    
}
