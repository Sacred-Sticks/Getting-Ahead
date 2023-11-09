using System.Collections.Generic;
using Kickstarter.Inputs;
using UnityEngine;

namespace Kickstarter.Identification
{
    public class Player : MonoBehaviour
    {
        public enum PlayerIdentifier
        {
            None,
            KeyboardAndMouse,
            ControllerOne,
            ControllerTwo,
            ControllerThree,
            ControllerFour,
        }

        [SerializeField] private PlayerIdentifier playerID;

        private IInputReceiver[] inputReceivers;
        private readonly List<IInputReceiver> registeredInputs = new List<IInputReceiver>();

        public PlayerIdentifier PlayerID
        {
            get
            {
                return playerID;
            }
            set
            {
                foreach (var inputReceiver in inputReceivers)
                    if (inputReceiver is not SkeletonController)
                        inputReceiver.UnsubscribeToInputs(this);
                playerID = value;
                foreach (var inputReceiver in inputReceivers)
                    if (inputReceiver is not SkeletonController)
                        inputReceiver.SubscribeToInputs(this);
            }
        }

        private void Awake()
        {
            inputReceivers = GetComponents<IInputReceiver>();
        }

        private void Start()
        {
            RegisterAllInputs();
        }

        private void OnEnable()
        {
            RegisterAllInputs();
        }

        private void OnDisable()
        {
            DeregisterAllInputs();
        }

        private void OnDestroy()
        {
            DeregisterAllInputs();
        }

        private void RegisterAllInputs()
        {
            foreach (var inputReceiver in inputReceivers)
            {
                if (registeredInputs.Contains(inputReceiver))
                    continue;
                inputReceiver.SubscribeToInputs(this);
                registeredInputs.Add(inputReceiver);
            }
        }

        private void DeregisterAllInputs()
        {
            foreach (var inputReceiver in inputReceivers)
            {
                if (!registeredInputs.Contains(inputReceiver))
                    continue;
                inputReceiver.UnsubscribeToInputs(this);
                registeredInputs.Remove(inputReceiver);
            }
        }
    }
}