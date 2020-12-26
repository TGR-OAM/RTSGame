using UnityEngine;

namespace UnitsControlScripts
{
    public class FractionMember : MonoBehaviour
    {
        [SerializeField]
        public Fraction fraction;
        void Start()
        {
        }
    }

    public enum Fraction
    {
        Player,
        Enemy
    }
}