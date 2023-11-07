using System;
using Kickstarter.Events;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.StateControllers;
using Unity.Mathematics;
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
    [Space]
    [SerializeField] private PlayerCharacterPairing[] players;

    public PlayerCharacterPairing[] Players
    {
        get
        {
            return players;
        }
    }

    private StateMachine<GameState> stateMachine;
    private LayOutRooms roomLayoutGenerator;
    private CameraManager cameraManager;

    public static GameManager instance;

    private (int x, int y) roomIndex;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameWin,
        GameLose,
    }

    #region Unity Events
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

    private void Awake()
    {
        InitializeSingleton();

        stateMachine = new StateMachine<GameState>.Builder()
            .WithInitialState(initialGameState)
            .WithTransition(GameState.MainMenu, GameState.Playing)
            .WithTransition(GameState.Playing, GameState.Paused)
            .WithTransition(GameState.Playing, GameState.GameWin)
            .WithTransition(GameState.Playing, GameState.GameLose)
            .WithTransition(GameState.Paused, GameState.Playing)
            .WithTransition(GameState.GameWin, GameState.MainMenu)
            .WithTransition(GameState.GameLose, GameState.GameWin)
            .Build();

        inputManager.Initialize(out int numPlayers);

        roomLayoutGenerator = GetComponent<LayOutRooms>();
        cameraManager = GetComponent<CameraManager>();
    }
    #endregion

    private void InitializeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene == SceneManager.GetSceneByBuildIndex(mainMenuIndex))
        {
            return;
        } // Main Menu Scene
        for (int i = gameplayStartIndex; i < gameplayEndIndex + 1; i++)
        {
            if (scene != SceneManager.GetSceneByBuildIndex(i))
                continue;
            InitializeLevel();
            SpawnPlayers(players);
            return;
        } // Gameplay Scenes
        if (scene == SceneManager.GetSceneByBuildIndex(endGameIndex))
        {
            return;
        } // Game End Scene
    }

    private void InitializeLevel()
    {
        var initialRoom = roomLayoutGenerator.InitializeLayout(out var initialRoomIndex);
        cameraManager.SetupCameraDictionary(initialRoom);
        roomIndex = initialRoomIndex;
    }

    private void SpawnPlayers(PlayerCharacterPairing[] playerCharacters)
    {
        for (int index = 0; index < playerCharacters.Length; index++)
        {
            var playerCharacter = playerCharacters[index];
            if (!playerCharacter.Body || !playerCharacter.Head)
                return;
            var spawnOrigin = new Vector3(roomIndex.x * 15, 0, roomIndex.y * 15);
            var body = Instantiate(playerCharacter.Body, spawnOrigin, quaternion.identity);
            body.name = $"b.{index + 1}";
            var head = Instantiate(playerCharacter.Head, spawnOrigin, quaternion.identity);
            head.name = $"h.{index + 1}";
            head.TryGetComponent(out Player headPlayer);
            head.TryGetComponent(out SkeletonController skeletonController);
            if (!headPlayer || !skeletonController)
                return;
            headPlayer.PlayerID = playerCharacter.PlayerID;
            skeletonController.Recapitate(body);
        }
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
        roomIndex = (roomIndex.x + (int)roomDirection.x, roomIndex.y + (int)roomDirection.y);
    }

    [Serializable]
    public class PlayerCharacterPairing
    {
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject head;
        [SerializeField] private Player.PlayerIdentifier playerID;

        public GameObject Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }
        public GameObject Head
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
            }
        }
        public Player.PlayerIdentifier PlayerID
        {
            get
            {
                return playerID;
            }
        }
    }
}
