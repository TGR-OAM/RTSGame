using UnityEngine;

namespace UnitsControlScripts
{
    public class FractionMember : MonoBehaviour
    {
        [SerializeField]
        public Fraction fraction;
        // Start is called before the first frame update
        void Start()
        {
            EntitiesLister.enteties.Add(this.gameObject);
        }
    }

    public enum Fraction
    {
        Player,
        Enemy
    }
}