using System;
using Kickstarter.Events;
using Kickstarter.Inputs;
using Kickstarter.State_Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, Kickstarter.Events.IServiceProvider
{
    [Tooltip("Purely for testing, the Game Manager is a singleton so don't worry, you don't need to remove your Game Manager"
           + "just set it appropriately to whichever game state you are testing.")]
    [SerializeField] private GameState initialGameState;
    [Space]
    [SerializeField] private InputManager inputManager;
    [Space]
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int gameplayStartIndex;
    [SerializeField] private int gameplayEndIndex;
    [SerializeField] private int endGameIndex;
    [Space]
    [SerializeField] private Service onRoomChange;
    
    private StateMachine<GameState> state;

    private LayOutRooms roomLayoutGenerator;
    private CameraManager cameraManager;

    public static GameManager instance;

    private Vector2 roomIndex;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameWin,
        GameLose,
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        onRoomChange.Event += ImplementService;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        onRoomChange.Event -= ImplementService;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene == SceneManager.GetSceneByBuildIndex(mainMenuIndex))
        {
            return;
        } // Main Menu Scene
        for (int i = gameplayStartIndex; i < gameplayEndIndex + 1; i++)
        {
            var initialRoom = roomLayoutGenerator.InitializeLayout();
            cameraManager.SetupCameraDictionary(initialRoom);
            return;
        } // Level Scenes
        if (scene == SceneManager.GetSceneByBuildIndex(endGameIndex))
        {
            return;
        } // Game End Scene
    }

    private void Awake()
    {
        InitializeSingleton();

        state = new StateMachine<GameState>(initialGameState);

        inputManager.Initialize(out int numPlayers);

        roomLayoutGenerator = GetComponent<LayOutRooms>();
        cameraManager = GetComponent<CameraManager>();
    }

    private void InitializeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case CameraManager.RoomChangeArgs roomChangeArgs:
                UpdateRoomIndex(roomChangeArgs.RoomDirection);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateRoomIndex(Vector2 roomDirection)
    {
        roomIndex += roomDirection;
    }
}
