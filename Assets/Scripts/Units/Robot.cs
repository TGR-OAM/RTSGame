using System;
using System.Collections.Generic;
using System.Linq;
using MainMenu_DemoStartScripts;
using Orders.EntityOrder;
using UnityEngine;

namespace Units
{
    public class Robot : Warrior
    {
        [SerializeField]
        private Body Body;
        [SerializeField]
        private Wheels Wheels;
        [SerializeField]
        private Weapon[] Weapons;
        void Start()
        {
            base.Start();
            wheelsInit();
            bodyInit();
            weaponsInit();
        }

        void wheelsInit()
        {
            agent.speed = Wheels.speed;
            agent.acceleration = Wheels.acceleration;
            MaxHp += Wheels.hp;
            Debug.Log(MaxHp);
        }

        void bodyInit()
        {
            MaxHp += Body.hp;
            reachDistance = Body.reachDistance;
            visionDistance = Body.visionDistance;
            Debug.Log(MaxHp);
        }

        void weaponsInit()
        {
            float attackdist = 0;
            foreach (var VARIABLE in Weapons)
            {
                MaxHp += VARIABLE.hp;
                damagePerSecond += VARIABLE.damage;
                attackdist += VARIABLE.attackdistance;
                Debug.Log(MaxHp);
            }

            attackdist /= Weapons.Length;
            attackDistance = attackdist;
            
        }
        protected override void BaseOrderListInitialization()
        {
            if (EntityLoader.Contain(GetType()))
            {
                List<GameOrderInitParams> gameOrderInitParams =
                    EntityLoader.GetOrderInitParamsFromDictionary(GetType()).OrderInitParams.Values.ToList();
                foreach (var g in gameOrderInitParams)
                {
                    foreach (var w in Weapons)
                    {
                        if (g is AttackOrderInitParams)
                            ((AttackOrderInitParams) g).whoAttacking += shooting => w.Shoot(shooting);
                    }
                    
                }
                orderableObject.SetPossibleOrderTypes(gameOrderInitParams);
            }
        }

        void Update()
        {
            
        }
    }
}
