using UnityEngine;

namespace Assets.Scripts.Orders.Buildings
{
    public class UnitCreationTask:GameOrder
    {
        private GameObject unitToCreate;
        
        public UnitCreationTask(GameObject unitToCreate, GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            this.unitToCreate = unitToCreate;
        }

        public override void StartOrder()
        {
            base.StartOrder();
        }

        public override void UpdateOrder()
        {
            
        }

        public override void StopOrder()
        {
            base.StopOrder();
        }
    }
}