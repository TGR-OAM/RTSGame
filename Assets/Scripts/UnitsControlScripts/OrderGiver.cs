using System;
using Buildings;
using HexWorldinterpretation;
using Orders;
using Orders.Units;
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

        public void GiveOrder(OrderableObject[] EntetiesToOrder, GameOrderType orderType, bool isIdleState = false)
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
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams();
                            moveOrderInitParams.destination = GetDestinationWithOffset(hit.point, EntetiesToOrder.Length);
                            GiveOrderToUnits(entety, moveOrderInitParams);
                        }
                    }
                    else
                    {

                        if (fraction != raycastHitGameObjectFraction.fraction)
                        {
                            AttackOrderInitParams attackOrderInitParams = new AttackOrderInitParams();
                            attackOrderInitParams.target = raycastHitGameObject;
                            GiveOrderToUnits(EntetiesToOrder, attackOrderInitParams);
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject enemyObject = hit.collider.gameObject;
                    FractionMember victimFraction = enemyObject.GetComponent<FractionMember>();

                    Building building = enemyObject.GetComponent<Building>();

                    if (EntetiesToOrder.Length == 1 && EntetiesToOrder[0].orderTypes.Contains(GameOrderType.Build))//if we selected one builder
                    {
                        BuildOrderInitParams buildOrderInitParams = new BuildOrderInitParams();
                        buildOrderInitParams.building = building;
                        GameOrder buildOrder = buildOrderInitParams.CreateOrder();
                        EntetiesToOrder[0].GiveOrder(buildOrder);
                        buildOrder.ObjectToOrder = EntetiesToOrder[0].gameObject;
                    }

                    if (inputHandler.PossibleOrders.Contains(GameOrderType.Attack))
                    {
                        if (victimFraction.fraction != fraction)
                        {
                            AttackOrderInitParams attackOrderInitParams = new AttackOrderInitParams();
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
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams();
                            moveOrderInitParams.destination = GetDestinationWithOffset(output, EntetiesToOrder.Length);
                            GiveOrderToUnits(EntetyToOrder, moveOrderInitParams);
                        }
                    }
                }
            }
            else
            {
                if (orderType == GameOrderType.MoveAttack)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveOrderInitParams moveAttackOrderInitParams = new MoveOrderInitParams();
                            moveAttackOrderInitParams.destination = GetDestinationWithOffset(hit.point, EntetiesToOrder.Length);
                            moveAttackOrderInitParams.isForMoveAttackOrder = true;
                            GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams);
                        }
                    }
                    else
                    {
                        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                        {
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                MoveOrderInitParams moveAttackOrderInitParams = new MoveOrderInitParams();
                                moveAttackOrderInitParams.destination = GetDestinationWithOffset(output, EntetiesToOrder.Length);
                                moveAttackOrderInitParams.isForMoveAttackOrder = true;
                                GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams);
                            }
                        }
                    }
                }

                if (orderType == GameOrderType.Move)
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams();
                            moveOrderInitParams.destination = GetDestinationWithOffset(output, EntetiesToOrder.Length);
                            GiveOrderToUnits(EntetyToOrder, moveOrderInitParams);
                            return;
                        }
                    }
                }

                if (orderType == GameOrderType.Attack)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, 100f, 1 << 8))
                    {
                        GameObject victim = hit.collider.gameObject;
                        FractionMember victimFraction = victim.GetComponent<FractionMember>();
                        if (victim.GetComponent<DamageSystem>() != null)
                        {
                            if (fraction != victimFraction.fraction)
                            {
                                AttackOrderInitParams currentParameters = new AttackOrderInitParams();
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