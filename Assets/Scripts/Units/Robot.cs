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
        }
        public void wheelsInit()
        {
            agent.speed = Wheels.speed;
            agent.acceleration = Wheels.acceleration;
            agent.stoppingDistance = Wheels.stopDistance;
            MaxHp += Wheels.hp;
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
        }
 
        public void bodyInit()
        {
            MaxHp += Body.hp;
            reachDistance = Body.reachDistance;
            visionDistance = Body.visionDistance;
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
        }

        public void weaponInit()
        {
            float attackdist = 0;
            foreach (Weapon weapon in Weapons)
            {
                MaxHp += weapon.hp;
                damagePerSecond += weapon.damage;
                attackdist += weapon.attackdistance;
            }

            attackdist /= Weapons.Length;
            attackDistance = attackdist;
            damageSystem.SetMaxHp(MaxHp);
            damageSystem.SetHpToMax();
        }
        protected override void BaseOrderListInitialization()
        {
            base.BaseOrderListInitialization();
            foreach (var weapon in Weapons)
            {
                WhoAttacking += shooting => weapon.Shoot(shooting);
            }
        }


    }
}
