using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseScript : MonoBehaviour, IInputReceiver
{
    UIDocument PauseUI;
    [SerializeField] FloatInput pauseInput;
    bool paused;
    private void Awake()
    {
        PauseUI = GetComponent<UIDocument>();
    }

    private void Start()
    {
        PauseUI.enabled = false;
        paused = false;
        SubscribeToInputs(null);
    }

    public void PauseGame()
    {
        paused = true;
        PauseUI.enabled = true;
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        paused = false;
        PauseUI.enabled = false;
        Time.timeScale = 1;
    }

    private void TogglePause(float input)
    {
        if (input == 0) return;
        if (!paused) PauseGame();
        else UnPauseGame();
    }

    public void SubscribeToInputs(Player player)
    {
        pauseInput.SubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerOne);
        pauseInput.SubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerTwo);
        pauseInput.SubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerThree);
        pauseInput.SubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerFour);
    }

    public void UnsubscribeToInputs(Player player)
    {
        pauseInput.UnsubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerOne);
        pauseInput.UnsubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerTwo);
        pauseInput.UnsubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerThree);
        pauseInput.UnsubscribeToInputAction(TogglePause, Player.PlayerIdentifier.ControllerFour);
    }
}
