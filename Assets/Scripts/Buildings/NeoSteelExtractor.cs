using System;
using GameResources;
using UnityEngine;

namespace Buildings
{
    public class ResourcesExtractor : Building
    {
        public GameResourceStruct Producition;
        public float TimeToProduce;

        private float curProductionTime;
        
        protected void Start()
        {
            base.Start();
        }

        protected void Update()
        {
            curProductionTime += Time.deltaTime;

            if (curProductionTime >= TimeToProduce)
            {
                
            }
        }
    }
}