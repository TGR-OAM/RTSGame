using System;
using System.ComponentModel.Design;
using Assets.Scripts.Buildings;
using Assets.Scripts.HexWorldinterpretation;
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
                    GameObject g = hit.collider.gameObject;
                    FractionMember f = g.GetComponent<FractionMember>();
                    if (g.GetComponent<DamageSystem>() == null)
                    {
                        foreach (OrderableObject entety in EntetiesToOrder)
                        {
                            MoveTask o = new MoveTask(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length), entety.gameObject);
                            entety.GiveOrder(o);
                        }
                    }
                    else
                    {
                        foreach (OrderableObject entety in EntetiesToOrder)
                        {
                            if (entety.GetComponent<FractionMember>() != f)
                            {
                                AttackTask t = new AttackTask(g, entety.gameObject);
                                entety.GiveOrder(t);
                            }
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject gameObject = hit.collider.gameObject;
                    FractionMember f = gameObject.GetComponent<FractionMember>();

                    Building building = gameObject.GetComponent<Building>();
                    
                    if (EntetiesToOrder.Length == 1 && EntetiesToOrder[0].orderTypes.Contains(typeof(BuildTask)))//if we selected one builder
                    {
                        BuildTask buildTask = new BuildTask(building, EntetiesToOrder[0].gameObject);
                        EntetiesToOrder[0].GiveOrder(buildTask);
                    }

                    if (inputHandler.PossibleOrders.Contains(typeof(AttackTask)))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            if (f == EntetyToOrder.GetComponent<FractionMember>())
                            {
                                AttackTask attackTask = new AttackTask(gameObject, EntetyToOrder.gameObject);
                                EntetyToOrder.GiveOrder(attackTask);
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
                            MoveTask o = new MoveTask(GetDestinationWithOffset(output, EntetiesToOrder.Length), EntetyToOrder.gameObject);
                            EntetyToOrder.GiveOrder(o);
                        }
                    }
                }

            }
            else
            {
                if (orderType == typeof(MoveAttackTask))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(hit.point, EntetiesToOrder.Length),
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
                                MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(output, EntetiesToOrder.Length),
                                    EntetyToOrder.gameObject);
                                EntetyToOrder.GiveOrder(o);
                            }
                        }
                    }
                }

                if (orderType == typeof(MoveTask))
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                    {
                        foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                        {
                            MoveTask o = new MoveTask(GetDestinationWithOffset(output, EntetiesToOrder.Length),
                                EntetyToOrder.gameObject);
                            EntetyToOrder.GiveOrder(o);
                        }
                    }
                }

                if (orderType == typeof(AttackTask))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, 100f, 1 << 8))
                    {
                        GameObject g = hit.collider.gameObject;
                        FractionMember f = g.GetComponent<FractionMember>();
                        if (g.GetComponent<DamageSystem>() != null)
                        {
                            foreach (OrderableObject EntetyToOrder in EntetiesToOrder)
                            {
                                if (EntetyToOrder.GetComponent<FractionMember>() != f)
                                {
                                    AttackTask t = new AttackTask(g, EntetyToOrder.gameObject);
                                    EntetyToOrder.GiveOrder(t);
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