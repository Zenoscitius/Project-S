﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeRotate : MonoBehaviour
{
    public Vector3 RotateAmount;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialize CubeRotate!");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotateAmount);
    }
}