using System;
using UnityEngine;
public enum PlayerActions
{
    Moving,
    Shooting,
    DamageTaken,
    DeReCap,
    STOP,
}

[Serializable]
public class AudioPairing<T> where T : Enum
{
    [SerializeField] private T state;
    [SerializeField] private float delay;
    [SerializeField] private AudioClip clip;
    public T State => state;
    public float Delay => delay;
    public AudioClip Clip => clip;

    
}