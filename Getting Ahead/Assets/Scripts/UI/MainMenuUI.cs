using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    Button buttonPlay;
    Button buttonQuit;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonPlay = root.Q<Button>("PlayButton");
        buttonQuit = root.Q<Button>("QuitButton");

        buttonPlay.RegisterCallback<ClickEvent>((evt) => gameManager.ChangeScene("play"));
        buttonQuit.RegisterCallback<ClickEvent>((evt) => Application.Quit());

        buttonPlay.RegisterCallback<NavigationSubmitEvent>((evt) => gameManager.ChangeScene("play"));
        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonPlay.RegisterCallback<FocusInEvent>(OnFocusIn);
        buttonPlay.RegisterCallback<FocusOutEvent>(OnFocusOut);
        buttonQuit.RegisterCallback<FocusInEvent>(OnFocusIn);
        buttonQuit.RegisterCallback<FocusOutEvent>(OnFocusOut);

        buttonPlay.RegisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Down: buttonQuit.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonQuit.RegisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Up: buttonPlay.Focus(); break;
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
