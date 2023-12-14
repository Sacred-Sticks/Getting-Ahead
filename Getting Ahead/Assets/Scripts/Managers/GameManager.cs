using System;
using System.Collections.Generic;
using Kickstarter.Events;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.StateControllers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Tooltip("Purely for testing, the Game Manager is a singleton so don't worry, you don't need to remove your Game Manager "
           + "just set it appropriately to whichever game state you are testing.")]
    [SerializeField] private GameState initialGameState;
    [Space]
    [SerializeField] private InputManager inputManager;
    [Space]
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int gameplayStartIndex;
    [SerializeField] private int gameplayEndIndex;
    [SerializeField] private int endGameIndex;
    [SerializeField] private int winGameIndex;
    [Space]
    [SerializeField] private Service onRoomChange;
    [Space]
    [SerializeField] private PlayerCharacterPairing[] players;

    [Header("In Engine Debugging")]
    [SerializeField] private bool useKeyboardMouse;

    public PlayerCharacterPairing[] Players { get; private set; }
    public int PlayerCount { get; private set; }

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
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        PlayerCount = useKeyboardMouse ? numPlayers : numPlayers - 1;

        roomLayoutGenerator = GetComponent<LayOutRooms>();
        cameraManager = GetComponent<CameraManager>();
    }

    private void Start()
    {
        CameraManager.OnRoomChange = onRoomChange;
    }
    #endregion

    private void InitializeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
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
        CameraManager.SetupCameraDictionary(initialRoom);
        roomIndex = initialRoomIndex;
    }

    private void SpawnPlayers(IReadOnlyList<PlayerCharacterPairing> playerCharacters)
    {
        Players = new PlayerCharacterPairing[PlayerCount];
        for (int i = useKeyboardMouse ? 0 : 1; i < (useKeyboardMouse ? PlayerCount : PlayerCount + 1); i++)
        {
            var playerCharacter = playerCharacters[Random.Range(0, playerCharacters.Count)];
            var playerID = playerCharacters[i].PlayerID;
            if (!playerCharacter.Body || !playerCharacter.Head)
                return;
            var spawnOrigin = new Vector3(roomIndex.x * 15, 0, roomIndex.y * 15);
            var body = Instantiate(playerCharacter.Body, spawnOrigin, quaternion.identity);
            body.name = $"b.{i + 1}";
            var head = Instantiate(playerCharacter.Head, spawnOrigin, quaternion.identity);
            head.name = $"h.{i + 1}";
            head.TryGetComponent(out Player headPlayer);
            head.TryGetComponent(out SkeletonController skeletonController);
            if (!headPlayer || !skeletonController)
                return;
            headPlayer.PlayerID = playerID;
            skeletonController.Recapitate(body);
            Players[(useKeyboardMouse ? i : i - 1)] = new PlayerCharacterPairing(head, body);
        }
    }

    public void UpdatePlayerBody(GameObject old, GameObject @new)
    {
        foreach (var playerCharacterPairing in Players)
        {
            if (playerCharacterPairing.Body != old)
                continue;
            playerCharacterPairing.Body = @new;
        }
    }

    public void ChangeScene(string sceneName)
    {
        int sceneIndex = sceneName switch
        {
            "MainMenu" => mainMenuIndex,
            "Gameplay" => gameplayStartIndex,
            "GameOver" => endGameIndex,
            "GoodEnding" => winGameIndex,
            _ => mainMenuIndex
        };
        SceneManager.LoadScene(sceneIndex);
    }

    #region Sub Classes
    [Serializable]
    public class PlayerCharacterPairing
    {
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject head;
        [SerializeField] private Player.PlayerIdentifier playerID;

        public PlayerCharacterPairing(GameObject head, GameObject body)
        {
            Head = head;
            Body = body;
        }

        public GameObject Body { get => body; set => body = value; }
        public GameObject Head { get => head; private set => head = value; }
        public Player.PlayerIdentifier PlayerID => playerID;
    }
    #endregion
}
