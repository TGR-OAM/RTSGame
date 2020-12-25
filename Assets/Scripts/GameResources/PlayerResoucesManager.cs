using GameResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameResources {
    public class PlayerResoucesManager : MonoBehaviour
    {
        [SerializeField]
        private Text[] resourcesTexts;

        [SerializeField]
        public GameResourceStruct playerResources { get; private set; }

        private void Start()
        {
            UpdateResourcesTexts();
        }

        public void UpdateResourcesTexts()
        {
            resourcesTexts[0].text = playerResources.neoSteel.ToString();
            resourcesTexts[1].text = playerResources.titanium.ToString();
            resourcesTexts[2].text = playerResources.unobtanium.ToString();
        }
    }
}
