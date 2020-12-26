using System;
using GameResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class ResourcesExtractor : Building
    {
        public GameResourceStruct Production;
        public float TimeToProduce;

        private float curProductionTime;
        private PlayerManager playerManager;
        protected void Start()
        {
            playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
            base.Start();
        }

        protected void Update()
        {
            curProductionTime += Time.deltaTime;

            if (curProductionTime >= TimeToProduce)
            {
                curProductionTime = 0;
                playerManager.PlayerResoucesManager.playerResources += Production;
            }
        }
    }
}