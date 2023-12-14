using System;
using Kickstarter.Events;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PauseTrigger : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput pauseInput;
    [SerializeField] private Service onTriggerPause;

    public void SubscribeToInputs(Player player)
    {
        pauseInput.SubscribeToInputAction(OnPauseInputChange, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        pauseInput.SubscribeToInputAction(OnPauseInputChange, player.PlayerID);
    }

    private void OnPauseInputChange(float input)
    {
        onTriggerPause.Trigger(new OnPauseTrigger(input));
    }

    public class OnPauseTrigger : EventArgs
    {
        public OnPauseTrigger(float pauseInput)
        {
            PauseInput = pauseInput;
        }
        
        public float PauseInput { get; }
    }
}
