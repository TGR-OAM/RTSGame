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
            foreach (var w in Weapons)
            {
                WhoAttacking += shooting => w.Shoot(shooting);
            }
        }

    }
}
