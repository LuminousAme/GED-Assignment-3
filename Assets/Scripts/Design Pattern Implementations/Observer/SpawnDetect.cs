using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//based on observer pattern video : https://youtu.be/UWMmib1RYFE

public class SpawnDetect : MonoBehaviour
{
    public static event Action ObjectChanged;

    private void OnEnable()
    {
        ObjectChanged?.Invoke();
    }

    private void OnDisable()
    {
        ObjectChanged?.Invoke();
    }
}
