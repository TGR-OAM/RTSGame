using Assets.Scripts.UnitsControlScripts;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Orders.Units
{
    public class AttackTask : GameOrder
    {
        public GameObject target;
        private Warrior WarriorToOrder;
        public AttackTask (GameObject t, GameObject ObjectToOrder):base(ObjectToOrder)
        {
            target = t;
        }

        public override void StartOrder()
        {
            base.StartOrder();
            if (ObjectToOrder.TryGetComponent(typeof(Unit), out Component component))
            {
                Warrior WarriorToOrder = component as Warrior;
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
                if (Vector3.Distance(WarriorToOrder.transform.position, target.transform.position) <= WarriorToOrder.attackDistance)
                {
                    StopOrder();
                    return;
                }
            }
        }

        public override void StopOrder()
        {
            base.StopOrder();
            if (WarriorToOrder!= null)
            {
                WarriorToOrder.agent.SetDestination(WarriorToOrder.transform.position);
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