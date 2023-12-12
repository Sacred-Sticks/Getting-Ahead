using Kickstarter.Events;
using UnityEngine;
using UnityEngine.UIElements;
using static PauseTrigger;

public class PauseUI : MonoBehaviour
{
    Button buttonResume;
    Button buttonQuit;
    [SerializeField] private Service onTriggerPause;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonResume = root.Q<Button>("ResumeButton");
        buttonQuit = root.Q<Button>("QuitButton");

        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonResume.RegisterCallback<NavigationMoveEvent>((evt) =>
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
                case NavigationMoveEvent.Direction.Up: buttonResume.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonResume.RegisterCallback<NavigationSubmitEvent>((evt) =>
        {
            Debug.Log("Test!");
            onTriggerPause.Trigger(new OnPauseTrigger(1));
        });

        buttonResume.Focus();
    }

    private void OnDisable()
    {
        buttonQuit.UnregisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());

        buttonResume.UnregisterCallback<NavigationMoveEvent>((evt) =>
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
                case NavigationMoveEvent.Direction.Up: buttonResume.Focus(); break;
            }
            evt.PreventDefault();
        });
        buttonResume.UnregisterCallback<NavigationSubmitEvent>((evt) =>
        {
            onTriggerPause.Trigger(new OnPauseTrigger(1));
        });
    }
}
