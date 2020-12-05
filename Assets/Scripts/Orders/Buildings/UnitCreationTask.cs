using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Orders.Buildings
{
    public class UnitCreationTask:GameOrder
    {
        private List<GameObject> unitPartsToCreate;
        private List<Transform> unitPartsTransform;
        
        private float TimeUntilConstruction;

        public UnitCreationTask(List<GameObject> unitPartsToCreate, List<Transform> unitPartsTransform,float timeToCreate,GameObject ObjectToOrder) : base(ObjectToOrder)
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
    }
}