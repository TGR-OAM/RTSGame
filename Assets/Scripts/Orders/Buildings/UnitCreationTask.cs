using UnityEngine;

namespace Assets.Scripts.Orders.Buildings
{
    public class UnitCreationTask:GameOrder
    {
        private GameObject unitToCreate;
        private float TimeUntilConstruction;

        public UnitCreationTask(GameObject unitToCreate, float timeToCreate,GameObject ObjectToOrder) : base(ObjectToOrder)
        {
            this.unitToCreate = unitToCreate;

            TimeUntilConstruction = timeToCreate;
        }

        public override void UpdateOrder()
        {
            TimeUntilConstruction -= Time.deltaTime;

            if (TimeUntilConstruction <= 0)
            {
                GameObject.Instantiate(unitToCreate);
                StopOrder();
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
        }
    }
}