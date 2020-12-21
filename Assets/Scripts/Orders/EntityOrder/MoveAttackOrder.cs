using Assets.Scripts.UnitsControlScripts;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class MoveAttackOrder : GameOrder
    {
        private Vector3 destination;
        private GameObject target;
        private Warrior UnitToOrder;

        public MoveAttackOrder(Vector3 destination)
        {
            this.destination = destination;
        }

        public override void StartOrder()
        {
            
            if (ObjectToOrder.TryGetComponent(typeof(Warrior), out Component component))
            {
                base.StartOrder();
                Warrior unit = component as Warrior;
                UnitToOrder = unit;
                unit.agent.isStopped = false;
                unit.agent.SetDestination(destination);
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
                    Attack(UnitToOrder);
                }
                
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
            foreach (GameObject g in thisUnit.fractionMember.lister.enteties)
            {
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

        private void Attack(Warrior thisUnit)
        {
            thisUnit.transform.LookAt(target.transform.position);
            thisUnit.agent.SetDestination(thisUnit.gameObject.transform.position);
            target.GetComponent<DamageSystem>().TakeDamage(thisUnit.damagePerSecond * Time.deltaTime);
        }

        #endregion
    }
}
