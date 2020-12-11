using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.UnitsControlScripts
{
    public class DamageSystem : MonoBehaviour
    {
        [SerializeField]
        public float Hp { get; private set; } = 100;
        [SerializeField]
        public float MaxHp{ get; private set; }

        public void TakeDamage(float value)
        {
            Hp -= value;
            if (Hp < 0)
            {
                Destroy(gameObject);
            }
        }
        
        public void Heal(float value)
        {
            Hp = Mathf.Clamp(Hp + value, 0, MaxHp);
        }

        public void SetMaxHpd(float MaxHp)
        {
            this.MaxHp = MaxHp;
        }

        public void SetHpToMax()
        {
            Hp = MaxHp;
        }
        
    }
}
