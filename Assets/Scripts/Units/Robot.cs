using System;
using System.Collections.Generic;
using System.Linq;
using MainMenu_DemoStartScripts;
using Orders.EntityOrder;
using UnityEngine;

namespace Units
{
    public class Robot : Unit
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
            foreach (var w in Weapons)
            {
                w.OrderableObject = orderableObject;
            }
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
                        if ((AttackOrderInitParams) g != null)
                            ((AttackOrderInitParams) g).whoAttacking += shooting => w.Shoot(shooting);
                    }
                    
                }
            }
        }

        void Update()
        {
        
        }
    }
}
