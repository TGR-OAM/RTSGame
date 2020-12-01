using Assets.Scripts.UnitsControlScripts;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class AttackTask : GameOrder
    {
        public GameObject target;
        public AttackTask (GameObject t, GameObject ObjectToOrder):base(ObjectToOrder)
        {
            target = t;
        }

        public override void StartOrder()
        {
            base.StartOrder();
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Unit unit = component as Unit;
                unit.agent.SetDestination(target.transform.position);
            }
        }

        public override void UpdateOrder()
        {
            if (target == null)
            {
                StopOrder();
                return;
            }
        
            if (ObjectToOrder.TryGetComponent(typeof(Warrior), out Component component))
            {
                Warrior unit = component as Warrior;
                if (Vector3.Distance(unit.transform.position, target.transform.position) <= unit.attackDistance)
                {
                    StopOrder();
                    return;
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
                return;
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