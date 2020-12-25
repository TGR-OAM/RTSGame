using ErrorReport;
using Units;
using UnitsControlScripts;
using UnityEngine;

namespace Orders.EntityOrder
{
    public class AttackOrderInitParams : GameOrderInitParams
    {
        public AttackOrderInitParams(string orderName) : base(orderName)
        {
        }
    }
    
    public class AttackOrderVariableParams : GameOrderVariableParams
    {
        public GameObject target;
        
        public AttackOrderVariableParams(GameObject target, GameObject objectToOrder) : base(objectToOrder)
        {
            this.target = target;
        }
    }
    
    public class AttackOrder : GameOrder
    {
        public GameObject target;
        private Warrior WarriorToOrder;
        public AttackOrder (AttackOrderVariableParams orderVariableParams) :base(orderVariableParams)
        {
            target = orderVariableParams.target;
        }

        public override void StartOrder()
        {
            if (ObjectToOrder.TryGetComponent(typeof(Warrior), out Component component))
            {
                base.StartOrder();
                WarriorToOrder = component as Warrior;
                WarriorToOrder.agent.isStopped = false;
                WarriorToOrder.agent.SetDestination(target.transform.position);
            }
        }

        public override void UpdateOrder()
        {
            if (target == null)
            {
                StopOrder();
                return;
            }

            if (WarriorToOrder != null)
            {
                if (WarriorToOrder.isNearToDestination(target.transform.position, WarriorToOrder.attackDistance))
                {
                    Attack(WarriorToOrder);
                    WarriorToOrder.agent.isStopped = true;
                }
                else
                {
                    WarriorToOrder.agent.isStopped = false;
                }
            }
        }
    
        public override void StopOrder()
        {
            base.StopOrder();
            if (WarriorToOrder!= null)
            {
                WarriorToOrder.agent.isStopped = true;
            }
        }
    
        #region Additional methods
    
        private void Attack(Warrior thisUnit)
        {
            if(thisUnit == null) return;
            thisUnit.transform.LookAt(target.transform.position);
            target.GetComponent<DamageSystem>().TakeDamage(thisUnit.damagePerSecond * Time.deltaTime);
        }
    
        #endregion
    }
}