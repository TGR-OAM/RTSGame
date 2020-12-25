using System;
using GameResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PLayerScripts {

    public class PlayerResoucesManager : MonoBehaviour
    {

        [SerializeField] public GameResourceStruct playerResources;
        
        public VoidWithGameResourceStruct OnUpdateResources;

        private void Update()
        {
            UpdateResourcesTexts();
        }

        public void UpdateResourcesTexts()
        {
            OnUpdateResources(playerResources);
        }
    }
}
