using HexWorldinterpretation;
using Orders.Units;
using UnitsControlScripts;
using UnityEngine;

namespace AIScripts
{
    public class SimpleAI : MonoBehaviour
    {
        [SerializeField]
        private HexGrid hexGrid;
        [SerializeField]
        private EntetiesLister unitLister;
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
                AttackPlayer();
                currentTimeFromAttack = 0;
            }
        }
    
        private void AttackPlayer()
        {
            print("Go go go");
            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams();
            moveOrderInitParams.destination = enemyBasePosition;
            moveOrderInitParams.isForMoveAttackOrder = true;
            foreach(GameObject gameObject in unitLister.enteties)
            {
                if (gameObject.GetComponent<FractionMember>().fraction == Fraction.Enemy)
                {
                    OrderGiver.GiveOrderToUnits(gameObject.GetComponent<OrderableObject>(), moveOrderInitParams);
                }
            }
        }
    }
}
