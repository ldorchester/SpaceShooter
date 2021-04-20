﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    [SerializeField] private float _speed = 10.0f;


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

       if (transform.position.y < -6.4f)
       {
           Destroy(this.gameObject);
       }
    }

}
