using System.Collections.Generic;
using System.Linq;
using Kickstarter.Inputs;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Purely for testing, the Game Manager is a singleton so don't worry, you don't need to remove your Game Manager"
           + "just set it appropriately to whichever game state you are testing.")]
    [SerializeField] private GameStateController.GameState initialGameState;

    [SerializeField] private InputManager inputManager;

    public static GameManager instance;

    private GameStateController gameStateController;

    private void Awake()
    {
        InitializeSingleton();

        gameStateController = new GameStateController(initialGameState);

        inputManager.Initialize();
    }

    private void InitializeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }

    private class GameStateController
    {
        public GameStateController(GameState initialGameState)
        {
            state = initialGameState;

            InitializeStateTransitions();
        }

        private Dictionary<GameState, GameState[]> StateTransitions;

        private GameState state;

        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                if (StateTransitions[State].Contains(value))
                    state = value;
            }
        }

        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameWon,
            GameLost,
        }

        private void InitializeStateTransitions()
        {
            StateTransitions = new Dictionary<GameState, GameState[]>()
            {
                {
                    GameState.MainMenu,
                    new GameState[]
                    {
                        GameState.Playing,
                    }
                },
                {
                    GameState.Playing,
                    new GameState[]
                    {
                        GameState.Paused,
                        GameState.GameLost,
                        GameState.GameWon,
                        GameState.MainMenu,
                    }
                },
                {
                    GameState.Paused,
                    new GameState[]
                    {
                        GameState.Playing,
                        GameState.MainMenu,
                    }
                },
                {
                    GameState.GameLost,
                    new GameState[]
                    {
                        GameState.MainMenu,
                    }
                },
                {
                    GameState.GameWon,
                    new GameState[]
                    {
                        GameState.MainMenu,
                    }
                },
            };
        }
    }
}
