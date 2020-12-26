using UnityEngine;
using UnityEngine.Serialization;

namespace UnitsControlScripts
{
    public class DamageSystem : MonoBehaviour
    {
        [SerializeField] private ProccessBar HealthBar;

        [SerializeField]
        public float Hp { get; private set; }
        public float MaxHp{ get; private set; }

        public void TakeDamage(float value)
        {
            Hp -= value;
            if (Hp < 0)
            {
                EntitiesLister.UpdateAllEnteties();
                Destroy(gameObject);
            }
            if(HealthBar != null) HealthBar.SetBarValue(Hp/MaxHp);
        }
        
        public void Heal(float value)
        {
            Hp = Mathf.Clamp(Hp + value, 0, MaxHp);
            if(HealthBar != null) HealthBar.SetBarValue(Hp/MaxHp);
        }

        public void SetMaxHp(float MaxHp)
        {
            this.MaxHp = MaxHp;
        }

        public void SetHpToMax()
        {
            Hp = MaxHp;
            if(HealthBar != null) HealthBar.SetBarValue(1f);
        }

        public void TrySetActiveHealthBar(bool isActive)
        {
            if(HealthBar != null)
                HealthBar.gameObject.SetActive(isActive);
        }

        public void TryUpdateHealthBar()
        {
            if(HealthBar != null)
                HealthBar.SetBarValue(0);
        }
        

    }
}
