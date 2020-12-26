using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitsControlScripts
{
    public static class EntitiesLister
    {
        public static List<GameObject> enteties { get; set; } = new List<GameObject>();

        public static void UpdateAllEnteties()
        {
            List<GameObject> newEntities = new List<GameObject>();
            foreach (var VARIABLE in enteties)
            {
                if (VARIABLE != null)
                {
                    newEntities.Add(VARIABLE);
                }
            }
        }
    }
}
