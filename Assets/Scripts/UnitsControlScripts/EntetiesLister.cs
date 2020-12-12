using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.UnitsControlScripts.UnitsControlScripts
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
