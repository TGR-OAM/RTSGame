using System;
using Buildings;
using HexWorldinterpretation;
using Orders;
using Orders.EntityOrder;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using System.Linq;

namespace UnitsControlScripts
{
    public class OrderGiver
    {
        public HexGrid hexGrid;
        public InputHandler inputHandler;
        public Fraction fraction = Fraction.Player;
        public OrderGiver(HexGrid grid, InputHandler inputHandler)
        {
            hexGrid = grid;
            this.inputHandler = inputHandler;
        }

        public void GiveOrder(OrderableObject[] EntetiesToOrder, GameOrderInitParams orderType, bool isIdleState = false)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (isIdleState)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
                {
                    GameObject raycastHitGameObject = hit.collider.gameObject;
                    FractionMember raycastHitGameObjectFraction = raycastHitGameObject.GetComponent<FractionMember>();
                    if (raycastHitGameObject.GetComponent<DamageSystem>() == null)
                    {
                        foreach (OrderableObject entety in EntetiesToOrder)
                        {
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams(GameOrderType.Move);
                            moveOrderInitParams.destination = GetDestinationWithOffset(hit.point, EntetiesToOrder.Length);
                            GiveOrderToUnits(entety, moveOrderInitParams);
                        }
                    }
                    else
                    {

                        if (fraction != raycastHitGameObjectFraction.fraction)
                        {
                            AttackOrderInitParams attackOrderInitParams = new AttackOrderInitParams(GameOrderType.Attack);
                            attackOrderInitParams.target = raycastHitGameObject;
                            GiveOrderToUnits(EntetiesToOrder, attackOrderInitParams);
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject enemyObject = hit.collider.gameObject;
                    FractionMember victimFraction = enemyObject.GetComponent<FractionMember>();

                    if (inputHandler.PossibleOrders.Any(x => x is AttackOrderInitParams))
                    {
                        if (victimFraction.fraction != fraction)
                        {
                            AttackOrderInitParams attackOrderInitParams = new AttackOrderInitParams(GameOrderType.Attack);
                            attackOrderInitParams.target = enemyObject;
                            GiveOrderToUnits(EntetiesToOrder, attackOrderInitParams);
                        }
                    }
                }
                else
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos),
                        out Vector3 output))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams(GameOrderType.Move);
                            moveOrderInitParams.destination = GetDestinationWithOffset(output, EntetiesToOrder.Length);
                            GiveOrderToUnits(EntetyToOrder, moveOrderInitParams);
                        }
                    }
                }
            }
            else
            {
                if (orderType is MoveAttackOrderInitParams)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveAttackOrderInitParams moveAttackOrderInitParams = new MoveAttackOrderInitParams(GameOrderType.MoveAttack);
                            moveAttackOrderInitParams.destination = GetDestinationWithOffset(hit.point, EntetiesToOrder.Length);
                            GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams);
                        }
                    }
                    else
                    {
                        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                        {
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                MoveAttackOrderInitParams moveAttackOrderInitParams = new MoveAttackOrderInitParams(GameOrderType.MoveAttack);
                                moveAttackOrderInitParams.destination = GetDestinationWithOffset(output, EntetiesToOrder.Length);
                                GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams);
                            }
                        }
                    }
                }

                if (orderType is MoveOrderInitParams)
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams(GameOrderType.Move);
                            moveOrderInitParams.destination = GetDestinationWithOffset(output, EntetiesToOrder.Length);
                            GiveOrderToUnits(EntetyToOrder, moveOrderInitParams);
                            return;
                        }
                    }
                }

                if (orderType is AttackOrderInitParams)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, 100f, 1 << 8))
                    {
                        GameObject victim = hit.collider.gameObject;
                        FractionMember victimFraction = victim.GetComponent<FractionMember>();
                        if (victim.GetComponent<DamageSystem>() != null)
                        {
                            if (fraction != victimFraction.fraction)
                            {
                                AttackOrderInitParams currentParameters = new AttackOrderInitParams(GameOrderType.MoveAttack);
                                currentParameters.target = victim;
                                GiveOrderToUnits(EntetiesToOrder, currentParameters);
                            }
                        }
                    }
                }
            }
        }

        public static void GiveOrderToUnits(OrderableObject[] orderableObjects, GameOrderInitParams gameOrderInitParams)
        {
            GameOrder order = gameOrderInitParams.CreateOrder();
            foreach (OrderableObject orderableObject in orderableObjects)
            {
                orderableObject.GiveOrder(order);
            }
        }

        public static void GiveOrderToUnits(OrderableObject orderableObject, GameOrderInitParams gameOrderInitParams)
        {
            GameOrder order = gameOrderInitParams.CreateOrder();
            orderableObject.GiveOrder(order);
        }

        private Vector3 GetDestinationWithOffset(Vector3 destination, int numOfUnits)
        {
            float offset = numOfUnits * 0.21f;
            return destination + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
        }

    }
}