using Assets.Scripts.Buildings;
using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.Orders;
using Assets.Scripts.Orders.Units;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UnitsControlScripts
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

        public void GiveOrder(OrderableObject[] EntetiesToOrder, Type orderType, bool isIdleState = false)
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
                            m
                            MoveOrder o = new MoveOrder(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length));
                            entety.GiveOrder(o);
                        }
                    }
                    else
                    {
                        foreach (OrderableObject entety in EntetiesToOrder)
                        {
                            if (entety.GetComponent<FractionMember>() != raycastHitGameObjectFraction)
                            {
                                AttackOrder order = new AttackOrder(raycastHitGameObject);
                                entety.GiveOrder(order);
                            }
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject enemyObject = hit.collider.gameObject;
                    FractionMember victimFraction = enemyObject.GetComponent<FractionMember>();

                    Building building = enemyObject.GetComponent<Building>();

                    if (EntetiesToOrder.Length == 1 && EntetiesToOrder[0].orderTypes.Contains(typeof(BuildOrder)))//if we selected one builder
                    {
                        BuildOrderInitParams buildOrderInitParams = new BuildOrderInitParams();
                        buildOrderInitParams.building = building;
                        GameOrder buildOrder = buildOrderInitParams.CreateOrder();
                        EntetiesToOrder[0].GiveOrder(buildOrder);
                        buildOrder.ObjectToOrder = EntetiesToOrder[0].gameObject;
                    }

                    if (inputHandler.PossibleOrders.Contains(typeof(AttackOrder)))
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
                if (orderType == typeof(MoveAttackOrder))
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

                if (orderType == typeof(MoveOrder))
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

                if (orderType == typeof(AttackOrder))
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