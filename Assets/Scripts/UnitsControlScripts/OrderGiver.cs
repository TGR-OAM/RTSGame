using Assets.Scripts.HexWorldinterpretation;
using Assets.Scripts.Orders.Units;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.UnitsControlScripts
{
    public class OrderGiver
    {
        hexGrid = grid;
    }
    public void GiveOrder (Unit[] units, OrderType orderType)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (orderType == OrderType.None)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, 1 << 8))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, 1 << 8))
                {
                    GameObject g = hit.collider.gameObject;
                    FractionMember f = g.GetComponent<FractionMember>();
                    if (g.GetComponent<DamageSystem>() == null)
                    {
                        foreach (Unit u in units)
                        {
                            float offset = units.Length * 0.21f;
                            MoveTask o = new MoveTask(hit.point + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)), u.gameObject);
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
                    if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(Input.mousePosition), out Vector3 output))
                    {
                        foreach (Unit u in units)
                        {
                            float offset = units.Length * 0.21f;
                            MoveTask o = new MoveTask(output + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)), u.gameObject);
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
                        float offset = units.Length * 0.21f;
                        MoveAttackTask o = new MoveAttackTask(hit.point + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)), u.gameObject);
                        u.orderableObject.GiveOrder(o);
                    }
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
                    float offset = units.Length * 0.21f;
                    MoveAttackTask o = new MoveAttackTask(hit.point + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)));
                    u.GiveOrder(o);
                }
            }
            else
            {
                if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(mousePos), out Vector3 output))
                {
                    foreach (Unit u in units)
                    {
                        foreach (Unit u in units)
                        {
                            float offset = units.Length * 0.21f;
                            MoveAttackTask o = new MoveAttackTask(output + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)),u.gameObject);
                            u.orderableObject.GiveOrder(o);
                        }
                    }
                }
            }
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