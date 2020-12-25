using System.Linq;
using HexWorldinterpretation;
using Orders;
using Orders.EntityOrder;
using Units;
using UnitsControlScripts;
using UnityEngine;

namespace AIScripts
{
    public class SimpleAI : MonoBehaviour
    {
        [SerializeField]
        private HexGrid hexGrid;
        [SerializeField]
        private float attackInterval;
        private float currentTimeFromAttack;
        [SerializeField]
        private Vector3 enemyBasePosition;

        private void Update()
        {
            currentTimeFromAttack += Time.deltaTime;
            if (currentTimeFromAttack >= attackInterval)
            {
                //AttackPlayer();
                currentTimeFromAttack = 0;
            }
        }
    
        private void AttackPlayer()
        {
            print("Go go go");
            SetMoveAttackOrderToUnits(EntitiesLister.enteties.Where(x =>
                    {
                        if (x == null)
                        {
                            return false;
                        }
                        x.TryGetComponent(typeof(FractionMember), out Component component);
                        return (component as FractionMember).fraction == Fraction.Enemy;
                    }
                )
                .Select(x => x.GetComponent<Warrior>()).ToArray(), enemyBasePosition);
            
        }

        public void SetAttackOrderToUnits(Warrior[] warriors, GameObject target)
        {
            foreach (Warrior warrior in warriors)
            {
                AttackOrderVariableParams attackOrderVariableParams = new AttackOrderVariableParams(target, warrior.gameObject);
                warrior.orderableObject.GiveOrder(new AttackOrder(attackOrderVariableParams));
            }
        }
        
        public void SetMoveAttackOrderToUnits(Warrior[] warriors, Vector3 destination)
        {
            foreach (Warrior warrior in warriors)
            {
                if(warrior == null) return;
                print(warrior);
                MoveAttackOrderVariableParams attackOrderVariableParams = new MoveAttackOrderVariableParams(destination, warrior.gameObject);
                warrior.orderableObject.GiveOrder(new MoveAttackOrder(attackOrderVariableParams));
            }
        }
        
        public void SetMoveOrderToUnits(Unit[] units, Vector3 destination)
        {
            foreach (Unit unit in units)
            {
                MoveOrderVariableParams moveOrderVariableParams = new MoveOrderVariableParams(destination, unit.gameObject);
                unit.orderableObject.GiveOrder(new MoveOrder(moveOrderVariableParams));
            }
        }
    }
}
