using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    Button buttonSafety;
    Button buttonQuit;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonSafety = root.Q<Button>("SafetyButton");
        buttonQuit = root.Q<Button>("QuitButton");

        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonSafety.RegisterCallback<NavigationMoveEvent>((evt) =>
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
                case NavigationMoveEvent.Direction.Up: buttonSafety.Focus(); break;
            }
            evt.PreventDefault();
        });

        buttonSafety.Focus();
    }

    private void OnDisable()
    {
        buttonQuit.UnregisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonSafety.UnregisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Down: buttonQuit.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonQuit.UnregisterCallback<NavigationMoveEvent>((evt) =>
        {
            switch (evt.direction)
            {
                case NavigationMoveEvent.Direction.Up: buttonSafety.Focus(); break;
            }
            evt.PreventDefault();
        });
    }
}
