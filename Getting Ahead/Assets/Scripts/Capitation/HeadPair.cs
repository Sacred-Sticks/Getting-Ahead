using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPair : MonoBehaviour
{
    public GameObject Head { get; set; }
    [SerializeField] private Transform headRoot;
    
    public Transform HeadRoot {
        get
        {
            return headRoot;
        }
    }
}
