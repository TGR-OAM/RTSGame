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

        public WhoAttacking WhoAttacking;
        void Start()
        {
            base.Start();
            RobotInits();
        }

        private void RobotInits()
        {
            Body.robot = this;
            Wheels.robot = this;
            foreach (var VARIABLE in Weapons)
            {
                VARIABLE.robot = this;
            }
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
            Debug.Log(damageSystem.Hp);
        }
        public void wheelsInit()
        {
            agent.speed = Wheels.speed;
            agent.acceleration = Wheels.acceleration;
            agent.stoppingDistance = Wheels.stopDistance;
            MaxHp += Wheels.hp;
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
            Debug.Log(damageSystem.Hp);
        }

        public void bodyInit()
        {
            MaxHp += Body.hp;
            reachDistance = Body.reachDistance;
            visionDistance = Body.visionDistance;
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
            Debug.Log(damageSystem.Hp);
        }

        public void weaponInit()
        {
            float attackdist = 0;
            foreach (var VARIABLE in Weapons)
            {
                MaxHp += VARIABLE.hp;
                damagePerSecond += VARIABLE.damage;
                attackdist += VARIABLE.attackdistance;
            }

            attackdist /= Weapons.Length;
            attackDistance = attackdist;
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
            Debug.Log(damageSystem.Hp);
        }
        protected override void BaseOrderListInitialization()
        {
            base.BaseOrderListInitialization();
            foreach (var w in Weapons)
            {
                WhoAttacking += shooting => w.Shoot(shooting);
            }
        }


    }
}
