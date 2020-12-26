using System;
using System.Timers;
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
        
        public AttackOrderVariableParams(GameObject target,GameObject objectToOrder) : base(objectToOrder)
        {
            this.target = target;
        }
    }
    
    public class AttackOrder : GameOrder
    {
        private float timer;
        private float currenttime = 0;
        public GameObject target;
        private Warrior WarriorToOrder;
        private AttackOrderVariableParams orderVariableParams;
        private float attackrecharge;

        public AttackOrder(AttackOrderInitParams attackOrderInitParams, AttackOrderVariableParams orderVariableParams) :
            base(orderVariableParams)
        {
            this.orderVariableParams = orderVariableParams;
            target = orderVariableParams.target;
        }

        public override void StartOrder()
        {
            timer = Time.time;
            if (ObjectToOrder.TryGetComponent(typeof(Warrior), out Component component))
            {
                base.StartOrder();
                WarriorToOrder = component as Warrior;
                WarriorToOrder.agent.isStopped = false;
                WarriorToOrder.agent.SetDestination(target.transform.position);
                attackrecharge = WarriorToOrder.attackrecharge;
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
                    if (currenttime - timer >= attackrecharge)
                    {
                        Attack(WarriorToOrder);
                        timer = Time.time;
                    }
                    else
                    {
                        currenttime = Time.time;
                    }
                        
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
            if(WarriorToOrder is Robot)
                (WarriorToOrder as Robot).WhoAttacking(new WhoAttakingDelegate());
            target.GetComponent<DamageSystem>().TakeDamage(thisUnit.damagePerSecond * Time.deltaTime);
        }
    
        #endregion
    }
    public delegate void WhoAttacking(WhoAttakingDelegate whoAttakingDelegate);

    public class WhoAttakingDelegate : EventArgs
    {
        
    }
}