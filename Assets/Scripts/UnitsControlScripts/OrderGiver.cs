using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderGiver
{
    private HexGrid hexGrid;
    public OrderGiver(HexGrid grid)
    {
        hexGrid = grid;
    }
    public void GiveOrder (Unit[] units, OrderType orderType)
    {
        if (orderType == OrderType.None)
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
                        MoveTask o = new MoveTask(hit.point + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)));
                        u.GiveOrder(o);
                    }
                }
                else
                {
                    AttackTask t = new AttackTask(g);
                    foreach (Unit u in units)
                    {
                        u.GiveOrder(t);
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
                        MoveTask o = new MoveTask(output + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)));
                        u.GiveOrder(o);
                    }
                }
            }
        }
        if (orderType == OrderType.MoveAttack)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, 1 << 8))
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
                if (hexGrid.TryRaycastHexGrid(Camera.main.ScreenPointToRay(Input.mousePosition), out Vector3 output))
                {
                    foreach (Unit u in units)
                    {
                        float offset = units.Length * 0.21f;
                        MoveAttackTask o = new MoveAttackTask(output + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)));
                        u.GiveOrder(o);
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
