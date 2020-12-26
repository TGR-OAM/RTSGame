using UnityEngine;
using Random = UnityEngine.Random;

namespace AIScripts
{
    public class UnitsCreator : MonoBehaviour
    {
        [SerializeField]
        private float unitCreatingInterval;
        private float timeFromPreviousCreation;
        [SerializeField]
        private Transform positionToSpawnUnits;
        [SerializeField]
        private GameObject WarriorToCreate;

        void Update()
        {
            timeFromPreviousCreation += Time.deltaTime;
            if (timeFromPreviousCreation >= unitCreatingInterval)
            {
                Instantiate(WarriorToCreate, positionToSpawnUnits.position, Quaternion.identity);
                
                timeFromPreviousCreation = 0;
            }
        }
    }
}
