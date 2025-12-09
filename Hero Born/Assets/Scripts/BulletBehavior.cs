using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BulletBehavior : MonoBehaviour
{
    public float OnscreenDlay = 3f;

    void Start()
    {
        Destroy(this.gameObject, OnscreenDlay);
    }

    void Update()
    {
        
    }
    
    
}
