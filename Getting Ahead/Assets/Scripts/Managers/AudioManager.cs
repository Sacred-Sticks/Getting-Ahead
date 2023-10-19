using System;
using Kickstarter.Events;
using UnityEngine;

public class AudioManager : MonoBehaviour, Kickstarter.Events.IServiceProvider
{
    [SerializeField] private Service onAudioTrigger;

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case AudioArgs audioArgs:
                audioArgs.target.GetComponent<AudioPlayer>().Play(audioArgs.audioName);
                break;
        }
    }

    private void OnEnable()
    {
        onAudioTrigger.Event += ImplementService;
    }
    private void OnDisable()
    {
        onAudioTrigger.Event -= ImplementService;
    }
    public class AudioArgs : EventArgs
    {
        public readonly GameObject target;
        public readonly string audioName;
        public AudioArgs(GameObject target, string audioName)
        {
            this.target = target;
            this.audioName = audioName;
        }
    }
}
