using Kickstarter.Inputs;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    public static GameManager instance;

    private void Awake()
    {
        InitializeSingleton();

        inputManager.Initialize();
    }

    private void InitializeSingleton()
    {

        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }

}
