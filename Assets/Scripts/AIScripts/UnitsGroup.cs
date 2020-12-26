using Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orders.EntityOrder;
using System;

public struct CheckPoint
{
    public GameObject Way;
    public string Name;
    public  Vector3 Position;
    public int CheckPointNumber;
    public CheckPoint(string name,Vector3 position,int checkPointNumber,GameObject way)
    {
        Way = way;
        Name = name;
        Position = position;
        CheckPointNumber = checkPointNumber;
    }
}
public class UnitsGroup
{
    public List<Unit> Members = new List<Unit>();
    public Vector3 GroupCenter = new Vector3(0, 0, 0);
    public CheckPoint Destination;
    public void ResetGroupCenter()
    {
        if (Members.Count == 0) return;
        GroupCenter = Members[0].transform.position;
        for(int i = 1;i < Members.Count; i++)
        {
            GroupCenter = (Members[i].transform.position - GroupCenter) / 2;
        }
    }

    public void AddGroupMember(Unit unit)
    {
        Members.Add(unit);
    }

    public void UpdateGroup()
    {
        CheckAchievingDestination();
        ThrowMembersToDestination();
    }
    private void CheckAchievingDestination()
    {
        foreach(Unit unit in Members)
        {
            if((Destination.Position - unit.transform.position).magnitude < 5)
            {
                GoToNextCheckPoint();
                return;
            } 
        }
    }

    private void GoToNextCheckPoint()
    {
        if (Destination.Way.transform.childCount == Destination.CheckPointNumber) return;
        string NextName = Destination.Name.Substring(0, 5);
        string PlusSubstring = (Destination.CheckPointNumber + 1).ToString();
        NextName += PlusSubstring;
        GameObject NextDestinationObject = Destination.Way.transform.FindChild(NextName).gameObject; 
        if(NextDestinationObject != null)
        {
            Destination = new CheckPoint(NextName, NextDestinationObject.transform.position, Destination.CheckPointNumber + 1, Destination.Way);
        }
            
    }

    private void ThrowMembersToDestination()
    {
        foreach (Warrior warrior in Members)
        {
            if (warrior.orderableObject.currentOrder is AttackOrder) return;
            foreach (Unit unit in Members)
            {
                if (unit.agent.destination == Destination.Position) continue;
                MoveAttackOrderVariableParams variableParams = new MoveAttackOrderVariableParams(Destination.Position, unit.gameObject);
                unit.orderableObject.GiveOrder(new MoveAttackOrder(variableParams));
            }
        }
    }

    public void FollowTheWay(GameObject way)
    {
        string Name = "Point1";
        Vector3 pointPosition = way.transform.FindChild(Name).transform.position;
        Destination = new CheckPoint(Name, pointPosition,1,way);
    }
}
