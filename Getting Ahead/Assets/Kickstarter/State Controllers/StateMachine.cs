using System;
using System.Collections.Generic;

namespace Kickstarter.State_Controllers
{
    public class StateMachine<TEnum> where TEnum : Enum
    {
        public StateMachine()
        {
            
        }

        private StateMachine(TEnum initialState, Dictionary<TEnum, List<TEnum>> transitions)
        {
            stateTransitions = transitions;
            currentState = initialState;
        }

        private readonly Dictionary<TEnum, List<TEnum>> stateTransitions;
        private TEnum currentState;

        public class Builder
        {
            private TEnum initialState;
            private readonly Dictionary<TEnum, List<TEnum>> transitions;

            public Builder()
            {
                transitions = new Dictionary<TEnum, List<TEnum>>();
            }

            public Builder WithInitialState(TEnum state)
            {
                initialState = state;
                return this;
            }

            public Builder WithTransition(TEnum fromState, TEnum toState)
            {
                if (!transitions.ContainsKey(fromState))
                    transitions.Add(fromState, new List<TEnum>());
                transitions[fromState].Add(toState);
                return this;
            }

            public StateMachine<TEnum> Build()
            {
                return new StateMachine<TEnum>(initialState, transitions);
            }
        }
    }
}
