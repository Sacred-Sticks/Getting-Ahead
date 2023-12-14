using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    Button buttonReturn;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonReturn = root.Q<Button>("ReturnButton");

        buttonReturn.RegisterCallback<NavigationSubmitEvent>((evt) =>
        {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        });
        buttonReturn.Focus();
    }
    
    private void OnDisable()
    {
        buttonReturn.UnregisterCallback<NavigationSubmitEvent>((evt) =>
        {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        buttonReturn.Focus();
    }
}
