using Units;
using UnitsControlScripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Orders.EntityOrder
{
    public class DefendOrderInitParams:GameOrderInitParams
    {
        public DefendOrderInitParams(string orderName) : base(orderName)
        {
        }
    }
    
    public class DefendOrderVariableParams:GameOrderVariableParams
    {
        public Vector3 PositionToDefend;
        
        public DefendOrderVariableParams(Vector3 PositionToDefend,GameObject objectToOrder) : base(objectToOrder)
        {
            this.PositionToDefend = PositionToDefend;
        }
    }

    public class DefendOrder:GameOrder
    {
        private float timer;
        private float currenttime = 0;
        private float attackrecharge;
        private Vector3 destination;
        private GameObject target;
        private Warrior UnitToOrder;

        public DefendOrder(DefendOrderVariableParams orderVariableParams) :base(orderVariableParams)
        {
            this.destination = orderVariableParams.PositionToDefend;
        }

        public override void StartOrder()
        {
            timer = Time.time;
            if (ObjectToOrder.TryGetComponent(typeof(Warrior), out Component component))
            {
                base.StartOrder();
                Warrior unit = component as Warrior;
                UnitToOrder = unit;
                unit.agent.isStopped = false;
                unit.agent.SetDestination(destination);
                attackrecharge = unit.attackrecharge;
            }
        }

        public override void UpdateOrder()
        {
            if (UnitToOrder != null) 
            {
                if (target == null)
                {
                    GameObject bestTarget = UpdateTarget(UnitToOrder);
                    if (bestTarget != null)
                    {
                        target = bestTarget;
                        UnitToOrder.agent.isStopped = false;
                        UnitToOrder.agent.SetDestination(bestTarget.transform.position);
                    }
                    else
                    {
                        if (UnitToOrder.agent.destination != destination)
                        {
                            UnitToOrder.agent.isStopped = false;
                            UnitToOrder.agent.SetDestination(destination);
                        }
                    }
                }
                else if (UnitToOrder.isNearToDestination(target.transform.position,UnitToOrder.attackDistance))
                {
                    if (currenttime - timer >= attackrecharge)
                    {
                        TryAttack(UnitToOrder);
                        timer = Time.time;
                    }
                    else
                    {
                        currenttime = Time.time;
                    }
                    
                    UnitToOrder.agent.isStopped = true;
                }
                else
                {
                    UnitToOrder.agent.isStopped = false;
                }
                currenttime = Time.time;
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                unit.agent.isStopped = true;
            }
        }

        #region Additional methods

        private GameObject UpdateTarget(Warrior thisUnit)
        {
            float smallestDst = float.MaxValue;
            GameObject o = null;
            foreach (GameObject g in EntitiesLister.enteties)
            {
                if (g == null) continue;
                float d = Vector3.Distance(thisUnit.transform.position, g.transform.position);
                if (g.GetComponent<FractionMember>().fraction != thisUnit.fractionMember.fraction && d < smallestDst &&
                    d < thisUnit.visionDistance)
                {
                    smallestDst = d;
                    o = g;
                }
            }

            return o;
        }

        private void TryAttack(Warrior thisUnit)
        {
           
            if(thisUnit == null) return;
            thisUnit.transform.LookAt(target.transform.position);
            target.GetComponent<DamageSystem>().TakeDamage(thisUnit.damagePerSecond);
            if(UnitToOrder is Robot)
                (UnitToOrder as Robot).WhoAttacking(new WhoAttakingDelegate());
        }
        #endregion
    }
}