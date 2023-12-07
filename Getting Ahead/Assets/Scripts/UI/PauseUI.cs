using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonResume = root.Q<Button>("ButtonResume");
        Button buttonQuit = root.Q<Button>("ButtonQuit");

        //buttonResume.RegisterCallback<ClickEvent>((evt) => gameManager.ChangeScene("play"));
        //TODO: MAKE THIS UNPAUSE!
        buttonQuit.RegisterCallback<ClickEvent>((evt) => Application.Quit());
        buttonQuit.RegisterCallback<NavigationSubmitEvent>((evt) => Application.Quit());
    }
}
