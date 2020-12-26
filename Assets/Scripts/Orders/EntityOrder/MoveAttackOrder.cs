using Units;
using UnitsControlScripts;
using UnityEngine;

namespace Orders.EntityOrder
{
    public class MoveAttackOrderInitParams : GameOrderInitParams
    {
        public MoveAttackOrderInitParams(string orderName) : base(orderName)
        {
        }
    }

    public class MoveAttackOrderVariableParams : GameOrderVariableParams
    {
        public Vector3 destination;
        
        public MoveAttackOrderVariableParams(Vector3 destination, GameObject objectToOrder) : base(objectToOrder)
        {
            this.destination = destination;
        }
    }
    
    public class MoveAttackOrder : GameOrder
    {
        private float timer;
        private float currenttime = 0;
        private Vector3 destination;
        private GameObject target;
        private Warrior UnitToOrder;
        private float attackrecharge;
        public MoveAttackOrder(MoveAttackOrderVariableParams orderVariableParams) :base(orderVariableParams)
        {
            this.destination = orderVariableParams.destination;
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
                if (UnitToOrder.isNearToDestination(destination,UnitToOrder.reachDistance))
                {
                    StopOrder();
                }

                if (target == null)
                {
                    GameObject bestTarget = UpdateTarget(UnitToOrder);
                    if (bestTarget != null)
                    {
                        target = bestTarget;
                        UnitToOrder.agent.SetDestination(bestTarget.transform.position);
                    }
                    else
                    {
                        if (UnitToOrder.agent.destination != destination)
                        {
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
