using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitsControlScripts
{
    public class EntetiesLister : MonoBehaviour
    {
        public List<GameObject> enteties;

        private void Update()
        {
            enteties = enteties.Where(x => x != null).ToList();
        }
    }
}
