using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonResume = root.Q<Button>("Resume");
        Button buttonQuit = root.Q<Button>("QuitButton");

        //buttonResume.RegisterCallback<ClickEvent>((evt) => gameManager.ChangeScene("play"));
        //TODO: MAKE THIS UNPAUSE!
        buttonQuit.RegisterCallback<ClickEvent>((evt) => Application.Quit());
    }
}
