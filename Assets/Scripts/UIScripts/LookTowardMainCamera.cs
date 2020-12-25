using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardMainCamera : MonoBehaviour
{
    public Camera lookTowardsTo;
    
    private void Start()
    {
        if(lookTowardsTo == null)
            lookTowardsTo = Camera.main;
    }

    void Update()
    {
        this.transform.rotation = (lookTowardsTo.transform.rotation);
    }
}
