﻿using System.Collections.Generic;
using UnityEngine;

namespace Orders.EntityOrder
{
    /*public class UnitCreationTask:GameOrder
    {
        private List<GameObject> unitPartsToCreate;
        private List<Transform> unitPartsTransform;
        
        private float TimeUntilConstruction;

        public UnitCreationTask(List<GameObject> unitPartsToCreate, List<Transform> unitPartsTransform,float timeToCreate)
        {
            this.unitPartsToCreate = unitPartsToCreate;
            this.unitPartsTransform = unitPartsTransform;

            TimeUntilConstruction = timeToCreate;
        }

        public override void UpdateOrder()
        {
            TimeUntilConstruction -= Time.deltaTime;
            
            if (TimeUntilConstruction <= 0)
            {
                CreateUnitFromParts();
                StopOrder();
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
        }

        void CreateUnitFromParts()
        {
            //create unit from part
        }
    }*/
}