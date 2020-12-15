using Assets.Scripts.UnitsControlScripts;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class AttackOrderInitParams : GameOrderInitParams
    {
        public GameObject target;
    }
    public class AttackOrder : GameOrder
    {
        public GameObject target;
        private Warrior WarriorToOrder;
        public AttackOrder (GameObject t)
        {
            target = t;
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
            thisUnit.transform.LookAt(target.transform.position);
            target.GetComponent<DamageSystem>().TakeDamage(thisUnit.damagePerSecond * Time.deltaTime);
        }
    
        #endregion
    }
}