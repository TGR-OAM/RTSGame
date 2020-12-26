using HexWorldinterpretation;
using UnitsControlScripts;
using UnitsControlScripts;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Units;
using Orders.EntityOrder;
public enum SimpleAIStates {
    Atacking,
    Defending
}

public class SimpleAI : MonoBehaviour
{
    [SerializeField]
    private Vector3 OurBasePosition;
    [SerializeField]
    private List<Unit> AIControllledUnits = new List<Unit>();
    private List<Unit> HostileUnits = new List<Unit>();
    private List<UnitsGroup> UnitsGroups = new List<UnitsGroup>();
    private SimpleAIStates CurrentState = SimpleAIStates.Defending;
    private Vector3 AtackTargetPosition; 
    [SerializeField]
    private Vector3 EnemiesBasePosition;

    public GameObject TriangleAtackPlan;

    private void Start()
    {
        OurBasePosition = gameObject.transform.position;
        AtackTargetPosition = EnemiesBasePosition;
        foreach(GameObject gameObject in EntitiesLister.enteties)
        {
            Unit unit = gameObject.GetComponent<Warrior>();
            OrderableObject orderableObject = gameObject.GetComponent<OrderableObject>();
            FractionMember fractionMember = gameObject.GetComponent<FractionMember>();
            if(unit != null && orderableObject != null && fractionMember != null)
            {
                if (fractionMember.fraction == Fraction.Enemy)
                {
                    AIControllledUnits.Add(unit);
                }
                else
                {
                    HostileUnits.Add(unit);
                }
            }
        }
    }
    private void Update()
    {
        /* currentTimeFromAttack += Time.deltaTime;
         if (currentTimeFromAttack >= attackInterval)
         {
             AttackPlayer();
             currentTimeFromAttack = 0;
         }*/
        foreach (Warrior warrior in AIControllledUnits)
        {
            EstimateSituationAround(warrior);
        }
    }
    
 

    

    private void EstimateSituationAround(Warrior warrior)
    {
        int FriendCounter = 0;
        int EnemiesCounter = 0;
        FindUnitsInVisionDistanceFromList(warrior, AIControllledUnits, ref FriendCounter);
        FindUnitsInVisionDistanceFromList(warrior, HostileUnits, ref EnemiesCounter);
        if(FriendCounter > EnemiesCounter)
        {
            DoAtackActions(warrior);
            foreach (UnitsGroup group in UnitsGroups) group.UpdateGroup();
        }
        else
        {
            DoDefenseAction(warrior, EnemiesCounter / FriendCounter);
        }

    }

    

    private void FindUnitsInVisionDistanceFromList(Warrior warrior,List<Unit> units,ref int counter)
    {
        foreach(Unit unit in units)
        {
            float distance = (unit.transform.position - warrior.transform.position).magnitude;
            if (distance < warrior.visionDistance) counter += 1;
        }

    }


    private void DoDefenseAction(Warrior warrior,float enemiesMajorityValue)
    {
        CurrentState = SimpleAIStates.Defending;
        GetGroupByUnit(warrior).ResetGroupCenter();
        Vector3 destination = GetGroupByUnit(warrior).GroupCenter;
        if(enemiesMajorityValue >= 3)
        {
            destination = OurBasePosition;
        }
        MoveOrderVariableParams moveOrderVariableParams = new MoveOrderVariableParams(destination, warrior.gameObject);
        warrior.orderableObject.GiveOrder(new MoveOrder(moveOrderVariableParams));
    }

    private void DoAtackActions(Warrior warrior)
    {
        if(CurrentState == SimpleAIStates.Defending)
        {
         AtackByTrianglePlan();
        }
        
    }

    private void SetGroups()
    {
        foreach(Warrior warrior in AIControllledUnits)
        {
            if (!IsGroupMember(warrior))
            {
                UnitsGroup group = new UnitsGroup();
                UnitsGroups.Add(group);
                group.AddGroupMember(warrior);
                SetGroupAroundUnit(warrior, ref group);

            }
        }

    }
    private void SetGroupAroundUnit(Warrior warrior,ref UnitsGroup group)
    {
        foreach(Warrior warriorFriend in AIControllledUnits)
        {
            float distance = (warrior.transform.position - warriorFriend.transform.position).magnitude;
            if (!IsGroupMember(warriorFriend) && distance < warrior.visionDistance / 3)
            {
                group.AddGroupMember(warriorFriend);
                SetGroupAroundUnit(warriorFriend, ref group);
            }
        }
    }

    private bool IsGroupMember(Unit unit)
    {
        foreach(UnitsGroup group in UnitsGroups)
        {
            foreach(Unit groupUnit in group.Members)
            {
                if (unit == groupUnit) return true;
            }
        }
        return false;
    }

    private UnitsGroup GetGroupByUnit(Unit unit)
    {
        foreach (UnitsGroup group in UnitsGroups)
        {
            foreach (Unit groupUnit in group.Members)
            {
                if (unit == groupUnit) return group;
            }
        }
        return null;
    }

    public void SetAttackOrderToUnits(Warrior[] warriors, GameObject target)
    {
        foreach (Warrior warrior in warriors)
        {
            AttackOrderVariableParams attackOrderVariableParams = new AttackOrderVariableParams(target, warrior.gameObject);
            warrior.orderableObject.GiveOrder(new AttackOrder(attackOrderVariableParams));
        }
    }

    public void SetMoveAttackOrderToUnits(List<Unit> units, Vector3 destination)
    {
        foreach (Unit unit in units)
        {
            if (unit == null) return;
            print(unit);
            MoveAttackOrderVariableParams attackOrderVariableParams = new MoveAttackOrderVariableParams(destination, unit.gameObject);
            unit.orderableObject.GiveOrder(new MoveAttackOrder(attackOrderVariableParams));
        }
    }

    public void SetMoveOrderToUnits(Unit[] units, Vector3 destination)
    {
        foreach (Unit unit in units)
        {
            MoveOrderVariableParams moveOrderVariableParams = new MoveOrderVariableParams(destination, unit.gameObject);
            unit.orderableObject.GiveOrder(new MoveOrder(moveOrderVariableParams));
        }
    }

    private void AtackByTrianglePlan()
    {
        CurrentState = SimpleAIStates.Atacking;
        SplitIntoGroups(AIControllledUnits, 3);
        /* Vector3 TargetForward = AtackTargetPosition;
         Vector3 ChangingVector = (TargetForward - UnitsGroups[1].GroupCenter) / 2;
         Vector3 TarggetLeft = new Vector3(-ChangingVector.z, ChangingVector.y, ChangingVector.x);
         Vector3 TargetRight = new Vector3(ChangingVector.z, ChangingVector.y, -ChangingVector.x);
         SetMoveAttackOrderToUnits(UnitsGroups[0].Members, TargetForward);
         SetMoveAttackOrderToUnits(UnitsGroups[1].Members, TarggetLeft);
         SetMoveAttackOrderToUnits(UnitsGroups[2].Members, TargetRight);*/
        UnitsGroups[0].FollowTheWay(TriangleAtackPlan.transform.FindChild("LeftWay").gameObject);
        UnitsGroups[1].FollowTheWay(TriangleAtackPlan.transform.FindChild("CenterWay").gameObject);
        UnitsGroups[2].FollowTheWay(TriangleAtackPlan.transform.FindChild("RightWay").gameObject);


    }

    private void SplitIntoGroups(List<Unit> units,int groupAmount)
    {
       // UnitsGroups = new List<UnitsGroup>();
        if (groupAmount > units.Count) return;
        int MembersInGroup = units.Count / groupAmount;
        for(int i = 0;i < groupAmount; i++)
        {
            UnitsGroup group = new UnitsGroup();
            for(int j = i * MembersInGroup;j < (i + 1) * MembersInGroup;j++)
            {
                group.AddGroupMember(units[j]);
            }
            UnitsGroups.Add(group);
        }
    }
}
