using System.ComponentModel.Design;
using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.Units;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.UnitsControlScripts
{
    public class OrderGiver
    {
        public HexGrid hexGrid;

        public OrderGiver(HexGrid grid)
        {
            hexGrid = grid;
        }

        public void GiveOrder(Unit[] units, OrderType orderType)
        {
            
            Vector2 mousePos = Mouse.current.position.ReadValue();
            
            if (orderType == OrderType.None)
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
                            AttackTask t = new AttackTask(g, u.gameObject);
                            u.orderableObject.GiveOrder(t);
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

            if (orderType == OrderType.MoveAttack)
            {
                if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                {
                    foreach (Unit u in units)
                    {
                        MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(output, units.Length), u.gameObject);
                        u.orderableObject.GiveOrder(o);
                    }
                }
            }


            if (orderType == OrderType.MoveAttack)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
                {
                    foreach (Unit u in units)
                    {
                        MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(hit.point, units.Length), u.gameObject);
                        u.orderableObject.GiveOrder(o);
                    }
                }
                else
                {
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                    {
                        foreach (Unit u in units)
                        {
                            MoveAttackTask o = new MoveAttackTask(GetDestinationWithOffset(output, units.Length), u.gameObject);
                            u.orderableObject.GiveOrder(o);
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

    public enum OrderType
    {
        Move,
        Attack,
        MoveAttack,
        None
    }
}