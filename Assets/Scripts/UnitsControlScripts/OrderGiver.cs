using System;
using Buildings;
using HexWorldinterpretation;
using Orders;
using Orders.EntityOrder;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using System.Linq;
using MainMenu_DemoStartScripts;
using Units;
using PLayerScripts;

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

        public void GiveOrderWithNonFixedParams(OrderableObject[] EntetiesToOrder, GameOrderInitParams orderType, bool isIdleState = false)
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
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams("");
                            MoveOrderVariableParams moveOrderVariableParams = new MoveOrderVariableParams(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length), null);
                            GiveOrderToUnits(entety, moveOrderInitParams,moveOrderVariableParams);
                        }
                    }
                    else
                    {
                        if (fraction != raycastHitGameObjectFraction.fraction)
                        {
                            AttackOrderInitParams attackOrderInitParams = EntityLoader.GetOrderInitParamsFromDictionary(typeof(Warrior))
                                .GetOrderInitParamsFromType(typeof(AttackOrderInitParams).ToString()) as AttackOrderInitParams;
                            AttackOrderVariableParams attackOrderVariableParams = new AttackOrderVariableParams(raycastHitGameObject, null);
                            GiveOrderToUnits(EntetiesToOrder, attackOrderInitParams, attackOrderVariableParams);
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject enemyObject = hit.collider.gameObject;
                    FractionMember victimFraction = enemyObject.GetComponent<FractionMember>();


                    if (victimFraction.fraction != fraction)
                    {
                        if (inputHandler.PossibleOrders.Any(x => x is AttackOrderInitParams))
                        {
                            AttackOrderInitParams attackOrderInitParams = EntityLoader.GetOrderInitParamsFromDictionary(typeof(Warrior))
                                .GetOrderInitParamsFromType(typeof(AttackOrderInitParams).ToString()) as AttackOrderInitParams;
                            AttackOrderVariableParams attackOrderVariableParams =
                                new AttackOrderVariableParams(enemyObject, null);
                            GiveOrderToUnits(EntetiesToOrder, attackOrderInitParams, attackOrderVariableParams);
                        }
                    }
                    else
                    {
                        if (inputHandler.PossibleOrders.Any(x => x is BuildOrderInitParams) && hit.transform.TryGetComponent(typeof(Building), out Component building))
                        {
                            foreach (OrderableObject entity in inputHandler.SelectedEnteties)
                            {
                                BuildOrderVariableParams buildOrderVariableParams = new BuildOrderVariableParams(building as Building, entity.gameObject);
                                entity.GiveOrder(new BuildOrder(buildOrderVariableParams));
                            }
                        }
                    }
                }
                else
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos),
                        out Vector3 output))
                    {
                        foreach (OrderableObject EntityToOrder in EntetiesToOrder)
                        {
                            if(EntityToOrder == null) continue;
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams("");
                            MoveOrderVariableParams moveOrderVariableParams = new MoveOrderVariableParams(GetDestinationWithOffset(output, EntetiesToOrder.Length), EntityToOrder.gameObject);
                            GiveOrderToUnits(EntityToOrder, moveOrderInitParams, moveOrderVariableParams);
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
                            MoveAttackOrderInitParams moveAttackOrderInitParams = new MoveAttackOrderInitParams("");
                            MoveAttackOrderVariableParams moveAttackOrderVariableParams =
                                new MoveAttackOrderVariableParams(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length), EntetyToOrder.gameObject);
                            GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams, moveAttackOrderVariableParams);
                        }
                    }
                    else
                    {
                        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                        {
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                MoveAttackOrderInitParams moveAttackOrderInitParams = new MoveAttackOrderInitParams("");
                                MoveAttackOrderVariableParams moveAttackOrderVariableParams =
                                    new MoveAttackOrderVariableParams(GetDestinationWithOffset(output, EntetiesToOrder.Length), EntetyToOrder.gameObject);
                                GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams, moveAttackOrderVariableParams);
                            }
                        }
                    }
                }
                
                if (orderType is DefendOrderInitParams)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            DefendOrderInitParams moveAttackOrderInitParams = new DefendOrderInitParams("");
                            DefendOrderVariableParams moveAttackOrderVariableParams =
                                new DefendOrderVariableParams(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length), EntetyToOrder.gameObject);
                            GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams, moveAttackOrderVariableParams);
                        }
                    }
                    else
                    {
                        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                        {
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                DefendOrderInitParams moveAttackOrderInitParams = new DefendOrderInitParams("");
                                DefendOrderVariableParams moveAttackOrderVariableParams =
                                    new DefendOrderVariableParams(GetDestinationWithOffset(output, EntetiesToOrder.Length), EntetyToOrder.gameObject);
                                GiveOrderToUnits(EntetyToOrder, moveAttackOrderInitParams, moveAttackOrderVariableParams);
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
                            MoveOrderInitParams moveOrderInitParams = new MoveOrderInitParams("");
                            MoveOrderVariableParams moveOrderVariableParams = new MoveOrderVariableParams(GetDestinationWithOffset(output, EntetiesToOrder.Length),EntetyToOrder.gameObject);
                            GiveOrderToUnits(EntetyToOrder, moveOrderInitParams, moveOrderVariableParams);
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
                                AttackOrderInitParams currentParameters =
                                    EntityLoader.GetOrderInitParamsFromDictionary(victim.GetComponent<Unit>().GetType())
                                        .GetOrderInitParamsFromType(typeof(AttackOrderInitParams).ToString()) as AttackOrderInitParams;
                                AttackOrderVariableParams attackOrderVariableParams =
                                    new AttackOrderVariableParams(victim,null);
                                GiveOrderToUnits(EntetiesToOrder, currentParameters, attackOrderVariableParams);
                            }
                        }
                    }
                }
            }
        }

        public static void GiveOrderToUnits(OrderableObject[] orderableObjects, GameOrderInitParams gameOrderInitParams, GameOrderVariableParams gameOrderVariableParams)
        {
            foreach (OrderableObject orderableObject in orderableObjects)
            {
                if(orderableObject == null) continue;
                gameOrderVariableParams.ObjectToOrder = orderableObject.gameObject;
                GameOrder order = gameOrderInitParams.CreateOrder(gameOrderVariableParams);
                orderableObject.GiveOrder(order);
            }
        }

        public static void GiveOrderToUnits(OrderableObject orderableObject, GameOrderInitParams gameOrderInitParams, GameOrderVariableParams gameOrderVariableParams)
        {
            if(orderableObject == null) return;
            GameOrder order = gameOrderInitParams.CreateOrder(gameOrderVariableParams);
            orderableObject.GiveOrder(order);
        }

        private Vector3 GetDestinationWithOffset(Vector3 destination, int numOfUnits)
        {
            float offset = numOfUnits * 0.21f;
            return destination + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
        }

    }
}