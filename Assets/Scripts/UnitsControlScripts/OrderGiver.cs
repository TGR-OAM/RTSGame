using System;
using System.ComponentModel.Design;
using Assets.Scripts.Buildings;
using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.Orders;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UnitsControlScripts
{
    public class OrderGiver
    {
        public HexGrid hexGrid;
        public InputHandler inputHandler;

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
                            MoveOrder o = new MoveOrder(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length), entety.gameObject);
                            entety.GiveOrder(o);
                        }
                    }
                    else
                    {
                        foreach (OrderableObject entety in EntetiesToOrder)
                        {
                            if (entety.GetComponent<FractionMember>() != raycastHitGameObjectFraction)
                            {
                                AttackOrder order = new AttackOrder(raycastHitGameObject, entety.gameObject);
                                entety.GiveOrder(order);
                            }
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject gameObject = hit.collider.gameObject;
                    FractionMember f = gameObject.GetComponent<FractionMember>();

                    Building building = gameObject.GetComponent<Building>();
                    
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
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            if (f == EntetyToOrder.GetComponent<FractionMember>())
                            {
                                AttackOrder attackOrder = new AttackOrder(gameObject, EntetyToOrder.gameObject);
                                EntetyToOrder.GiveOrder(attackOrder);
                            }
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
                            MoveOrder order = new MoveOrder(GetDestinationWithOffset(output, EntetiesToOrder.Length), EntetyToOrder.gameObject);
                            EntetyToOrder.GiveOrder(order);
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
                            MoveAttackOrder o = new MoveAttackOrder(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length),
                                EntetyToOrder.gameObject);
                            EntetyToOrder.GiveOrder(o);
                        }
                    }
                    else
                    {
                        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                        {
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                MoveAttackOrder o = new MoveAttackOrder(GetDestinationWithOffset(output, EntetiesToOrder.Length),
                                    EntetyToOrder.gameObject);
                                EntetyToOrder.GiveOrder(o);
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
                            MoveOrder o = new MoveOrder(GetDestinationWithOffset(output, EntetiesToOrder.Length),
                                EntetyToOrder.gameObject);
                            EntetyToOrder.GiveOrder(o);
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
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                if (EntetyToOrder.GetComponent<FractionMember>() != victimFraction)
                                {
                                    AttackOrderInitParams currentParameters = new AttackOrderInitParams();
                                    currentParameters.target = victim;
                                    GameOrder order = currentParameters.CreateOrder();
                                    order.ObjectToOrder = EntetyToOrder.gameObject;
                                    EntetyToOrder.GiveOrder(order);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Vector3 GetDestinationWithOffset(Vector3 destination, int numOfUnits)
        {
            float offset = numOfUnits * 0.21f;
            return destination + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
        }
        
    }
}