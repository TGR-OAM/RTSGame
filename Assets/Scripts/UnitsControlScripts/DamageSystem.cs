using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.UnitsControlScripts
{
    public class DamageSystem : MonoBehaviour
    {
        public float Hp;
        public float MaxHp;

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
