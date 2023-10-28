using System;
using System.Collections.Generic;

namespace Kickstarter.State_Controllers
{
    public class StateMachine<TState> where TState : Enum
    {
        public StateMachine(TState initialState)
        {
            var allStates = Enum.GetValues(typeof(TState));
            foreach (TState potentialState in allStates)
            {
                stateTransitions.Add(potentialState, new List<TState>());
                onStateBegin.Add(potentialState, null);
                onStateEnd.Add(potentialState, null);
            }

            State = initialState;
        }

        public enum StateChange
        {
            Begin,
            End,
        }

        private TState state;
        public TState State
        {
            get
            {
                return state;
            }
            set
            {
                if (!stateTransitions[State].Contains(value))
                    return;
                onStateEnd[state]();
                state = value;
                onStateBegin[state]();
            }
        }

        private readonly Dictionary<TState, List<TState>> stateTransitions = new Dictionary<TState, List<TState>>();
        private readonly Dictionary<TState, Action> onStateBegin = new Dictionary<TState, Action>();
        private readonly Dictionary<TState, Action> onStateEnd = new Dictionary<TState, Action>();

        public void AddTransition(TState baseState, TState newState)
        {
            stateTransitions[baseState].Add(newState);
        }

        public void SubscribeToStateChange(StateChange changeType, TState state, Action subscription)
        {
            switch (changeType)
            {
                case StateChange.Begin:
                    onStateBegin[state] += subscription;
                    break;
                case StateChange.End:
                    onStateEnd[state] += subscription;
                    break;
            }
        }

        public void UnsubscribeToStateChange(StateChange changeType, TState state, Action subscription)
        {
            switch (changeType)
            {
                case StateChange.Begin:
                    onStateBegin[state] -= subscription;
                    break;
                case StateChange.End:
                    onStateEnd[state] -= subscription;
                    break;
            }
        }
    }
}
