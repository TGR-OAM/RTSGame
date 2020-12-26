using System;
using UnityEngine;
using System.Collections;
 
public class ExplosionEnd : MonoBehaviour
{
    private  float currenttime = 0;
    private float timer;
    [SerializeField] private ParticleSystem ps;

    private void Start()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if (currenttime - timer >= ps.duration)
            Destroy(gameObject);
        else
            currenttime = Time.time;
    }
     
}