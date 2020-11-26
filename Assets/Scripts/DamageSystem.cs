using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField]
    private float hp;

    public void TakeDamage(float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
