using System;
using System.ComponentModel.Design;
using Assets.Scripts.Buildings;
using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.Units;
using UnityEditor.Animations;
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

        public void GiveOrder(Unit[] units, Type orderType, bool isIdleState = false)
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
                        foreach (Unit u in units)
                        {
                            MoveTask o = new MoveTask(GetDestinationWithOffset(hit.point, units.Length), u.gameObject);
                            u.orderableObject.GiveOrder(o);
                        }
                    }
                    else
                    {
                        foreach (Unit u in units)
                        {
                            if (u.fractionMember != f)
                            {
                                AttackTask t = new AttackTask(g, u.gameObject);
                                u.orderableObject.GiveOrder(t);
                            }
                        }
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 10))
                {
                    GameObject gameObject = hit.collider.gameObject;
                    FractionMember f = gameObject.GetComponent<FractionMember>();

                    Building building = gameObject.GetComponent<Building>();
                    
                    if (units.Length == 1 && units[0].orderableObject.orderTypes.Contains(typeof(BuildTask)))//if we selected one builder
                    {
                        BuildTask buildTask = new BuildTask(building, units[0].gameObject);
                        units[0].orderableObject.GiveOrder(buildTask);
                    }

                    if (inputHandler.PossibleOrders.Contains(typeof(AttackTask)))
                    {
                        foreach (Unit unit in units)
                        {
                            AttackTask attackTask = new AttackTask(gameObject,unit.gameObject);
                            unit.orderableObject.GiveOrder(attackTask);
                        }
                        
                    }
                }
                else
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos),
                        out Vector3 output))
                    {
                        foreach (Unit u in units)
                        {
                            MoveTask o = new MoveTask(GetDestinationWithOffset(output, units.Length), u.gameObject);
                            u.orderableObject.GiveOrder(o);
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
                        foreach (Unit u in units)
                        {
                            MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(hit.point, units.Length),
                                u.gameObject);
                            u.orderableObject.GiveOrder(o);
                        }
                    }
                    else
                    {
                        if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                        {
                            foreach (Unit u in units)
                            {
                                MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(output, units.Length),
                                    u.gameObject);
                                u.orderableObject.GiveOrder(o);
                            }
                        }
                    }
                }

                if (orderType == typeof(MoveTask))
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                    {
                        foreach (Unit u in units)
                        {
                            MoveTask o = new MoveTask(GetDestinationWithOffset(output, units.Length),
                                u.gameObject);
                            u.orderableObject.GiveOrder(o);
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
                            foreach (Unit u in units)
                            {
                                if (u.fractionMember != f)
                                {
                                    AttackTask t = new AttackTask(g, u.gameObject);
                                    u.orderableObject.GiveOrder(t);
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