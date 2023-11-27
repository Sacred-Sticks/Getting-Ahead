using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonPlay = root.Q<Button>("PlayButton");
        Button buttonQuit = root.Q<Button>("QuitButton");

        //buttonPlay.clicked += () => gameManager.ChangeScene("play");
        buttonPlay.RegisterCallback<ClickEvent>((evt) => gameManager.ChangeScene("play"));
        buttonQuit.RegisterCallback<ClickEvent>((evt) => Application.Quit());
    }
}
