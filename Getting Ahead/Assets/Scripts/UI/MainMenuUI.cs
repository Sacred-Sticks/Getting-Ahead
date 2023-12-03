using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
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

        buttonPlay.RegisterCallback<ClickEvent>((evt) => gameManager.ChangeScene("play"));
        buttonQuit.RegisterCallback<ClickEvent>((evt) => Application.Quit());

        buttonPlay.RegisterCallback<NavigationSubmitEvent>((evt) => gameManager.ChangeScene("play"));
        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonPlay.RegisterCallback<FocusInEvent>(OnFocusIn);
        buttonPlay.RegisterCallback<FocusOutEvent>(OnFocusOut);
        buttonQuit.RegisterCallback<FocusInEvent>(OnFocusIn);
        buttonQuit.RegisterCallback<FocusOutEvent>(OnFocusOut);
        buttonControls.RegisterCallback<FocusInEvent>(OnFocusIn);
        buttonControls.RegisterCallback<FocusOutEvent>(OnFocusOut);
        buttonCredits.RegisterCallback<FocusInEvent>(OnFocusIn);
        buttonCredits.RegisterCallback<FocusOutEvent>(OnFocusOut);

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

    private void OnFocusOut(FocusOutEvent evt)
    {
        Debug.Log(evt.currentTarget + "lost focus.");
    }

    private void OnFocusIn(FocusInEvent evt)
    {
        Debug.Log(evt.currentTarget + "is focused.");
    }

    private void Start()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("PlayButton").Focus();
    }
}
