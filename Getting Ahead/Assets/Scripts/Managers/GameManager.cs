using Kickstarter.Inputs;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        inputManager.Initialize();
    }

}
