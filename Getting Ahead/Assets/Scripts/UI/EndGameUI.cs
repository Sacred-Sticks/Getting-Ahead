using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    Button buttonMenu;
    Button buttonQuit;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonMenu = root.Q<Button>("MenuButton");
        buttonQuit = root.Q<Button>("QuitButton");


        buttonMenu.RegisterCallback<NavigationSubmitEvent>((evt) => GameManager.instance.ChangeScene("mainmenu"));
        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonMenu.RegisterCallback<NavigationMoveEvent>((evt) =>
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
                case NavigationMoveEvent.Direction.Up: buttonMenu.Focus(); break;
            }
            evt.PreventDefault();
        });
        
        buttonMenu.Focus();
    }

    private void OnDisable()
    {
        buttonMenu.UnregisterCallback<NavigationSubmitEvent>((evt) => GameManager.instance.ChangeScene("mainmenu"));
        buttonQuit.UnregisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonMenu.UnregisterCallback<NavigationMoveEvent>((evt) =>
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
                case NavigationMoveEvent.Direction.Up: buttonMenu.Focus(); break;
            }
            evt.PreventDefault();
        });

    }
    private void Start()
    {
        buttonMenu.Focus();
    }
}
