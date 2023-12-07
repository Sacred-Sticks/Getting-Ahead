using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsUI : MonoBehaviour
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

        
    }
    
    private void OnDisable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonReturn = root.Q<Button>("ReturnButton");

        buttonReturn.UnregisterCallback<NavigationSubmitEvent>((evt) =>
        {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("ReturnButton").Focus();
    }
}
