using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class TestTaskAssigner : MonoBehaviour
{
    [SerializeField]
    public List<Unit> units;
    [SerializeField]
    private Fraction fraction = Fraction.Player;
    [SerializeField]
    private RectTransform selectionBox;
    private Vector2 startPos;
    private UnitLister lister;
    [SerializeField]
    private string listerTag;

    [SerializeField]
    private List<Type> possibleOrderTypes = new List<Type>();

    Player player;
    bool isSelecting = false;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        lister = GameObject.FindGameObjectWithTag(listerTag).GetComponent<UnitLister>();
    }

    void Update()
    {
        if (player.playerData.state == PlayerOrderState.Idle)
        { 
            ControlUnits();
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) { startPos = Input.mousePosition; isSelecting = true; }
            if (Input.GetMouseButton(0) && isSelecting) UpdateSelectionBox(Input.mousePosition);
            if (Input.GetMouseButtonUp(0) && isSelecting) { ReleaseSelectionBox();}
        }
        else
        {
            if (isSelecting)
            {
                ReleaseSelectionBox();
            }
            startPos = Input.mousePosition;
            isSelecting = false;
            
        }
        
    }

    private void ReleaseSelectionBox()
    {
        isSelecting = false;
        selectionBox.gameObject.SetActive(false);
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        units.Clear();
        List<GameObject> guys = lister.units;
        foreach (GameObject unit in guys)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y && unit.GetComponent<FractionMember>().fraction == fraction)
            {
                units.Add(unit.GetComponent<Unit>());
            }
        }

        DefinePossibleOrderTypes();
    }

    private void DefinePossibleOrderTypes()
    {
        possibleOrderTypes = new List<Type>();
        foreach (Unit u in units)
        {
            possibleOrderTypes.AddRange(u.orderTypes);
        }
        foreach (Unit u in units)
        {
            List<Type> types = u.orderTypes;
            foreach (Type t in possibleOrderTypes)
            {
                if (!types.Contains(t))
                {
                    possibleOrderTypes.Remove(t);
                }
            }
        }
        for (int i = 0; i < possibleOrderTypes.Count; i++)
        {
            Type t = possibleOrderTypes[i];
            for (int j = i + 1; j < possibleOrderTypes.Count; j++)
            {
                if (t == possibleOrderTypes[j])
                {
                    possibleOrderTypes.RemoveAt(j);
                    j--;
                }
            }
        }

        player.UpdatePossibleOrders(possibleOrderTypes);
    }

    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeSelf)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    private void ControlUnits()
    {
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.A))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
            {
                GameObject g = hit.collider.gameObject;
                FractionMember f = g.GetComponent<FractionMember>();
                if (g.GetComponent<DamageSystem>() == null || (f != null && f.fraction == fraction))
                {
                    foreach (Unit u in units)
                    {
                        float offset = units.Count * 0.21f;
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
        }
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.A))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
            {
                foreach (Unit u in units)
                {
                    float offset = units.Count * 0.21f;
                    MoveAttackTask o = new MoveAttackTask(hit.point + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset)));
                    u.GiveOrder(o);
                }
            }
        }
    }
}
