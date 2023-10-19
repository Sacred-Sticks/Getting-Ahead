using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(string audioName)
    {
        AudioClip clip = audioClips.Where(a=>a.name==audioName).FirstOrDefault();
        audioSource.clip = clip;
        audioSource.Play();
    }

    [System.Serializable]
    struct StringToClip
    {
        [SerializeField] private AudioClip clip;

        public AudioClip Clip { get
            {
                return clip;
            } 
        }
    }
}
