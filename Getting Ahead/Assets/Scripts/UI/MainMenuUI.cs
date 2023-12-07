using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private GameObject ControlsMenu;
    Button buttonPlay;
    Button buttonQuit;
    Button buttonCredits;
    Button buttonControls;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonPlay = root.Q<Button>("PlayButton");
        buttonQuit = root.Q<Button>("QuitButton");
        buttonCredits = root.Q<Button>("CreditButton");
        buttonControls = root.Q<Button>("ControlButton");

        buttonPlay.RegisterCallback<NavigationSubmitEvent>((evt) => GameManager.instance.ChangeScene("play"));
        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());
        buttonCredits.RegisterCallback<NavigationSubmitEvent>((evt) =>
        {
            CreditsMenu.SetActive(true);
            gameObject.SetActive(false);
        });
        buttonControls.RegisterCallback<NavigationSubmitEvent>((evt) =>
        {
            ControlsMenu.SetActive(true);
            gameObject.SetActive(false);
        });

        buttonPlay.RegisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Down: buttonCredits.Focus(); break;
                case NavigationMoveEvent.Direction.Right: buttonControls.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonQuit.RegisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Up: buttonControls.Focus(); break;
                case NavigationMoveEvent.Direction.Left: buttonCredits.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonControls.RegisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Down: buttonQuit.Focus(); break;
                case NavigationMoveEvent.Direction.Left: buttonPlay.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonCredits.RegisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Up: buttonPlay.Focus(); break;
                case NavigationMoveEvent.Direction.Right: buttonQuit.Focus(); break;
            }
            evt.PreventDefault();
        });

    }

    private void OnDisable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonPlay.UnregisterCallback<ClickEvent>((evt) => GameManager.instance.ChangeScene("play"));
        buttonQuit.UnregisterCallback<ClickEvent>((evt) => Application.Quit());

        buttonPlay.UnregisterCallback<NavigationSubmitEvent>((evt) => GameManager.instance.ChangeScene("play"));
        buttonQuit.UnregisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonCredits.UnregisterCallback<NavigationSubmitEvent>((evt) => GameManager.instance.ChangeScene("credits"));
        buttonControls.UnregisterCallback<NavigationSubmitEvent>((evt) => GameManager.instance.ChangeScene("controls"));

        buttonPlay.UnregisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Down: buttonCredits.Focus(); break;
                case NavigationMoveEvent.Direction.Right: buttonControls.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonQuit.UnregisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Up: buttonControls.Focus(); break;
                case NavigationMoveEvent.Direction.Left: buttonCredits.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonControls.UnregisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Down: buttonQuit.Focus(); break;
                case NavigationMoveEvent.Direction.Left: buttonPlay.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonCredits.UnregisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Up: buttonPlay.Focus(); break;
                case NavigationMoveEvent.Direction.Right: buttonQuit.Focus(); break;
            }
            evt.PreventDefault();
        });

    }
    private void Start()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("PlayButton").Focus();
    }
}
