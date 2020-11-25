﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        lister = GameObject.FindGameObjectWithTag(listerTag).GetComponent<UnitLister>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlUnits();
        if (Input.GetMouseButtonDown(0)) startPos = Input.mousePosition;
        if (Input.GetMouseButton(0)) UpdateSelectionBox(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) ReleaseSelectionBox();
    }

    private void ReleaseSelectionBox()
    {
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
    }

    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
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
