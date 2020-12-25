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
