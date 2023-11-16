using Kickstarter.Categorization;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.Events;

public class Transitioner : MonoBehaviour
{
    [SerializeField] private Direction direction;
    [SerializeField] private CategoryType playerType;
    [SerializeField] private CinemachineVirtualCamera roomCamera;
    [SerializeField] private UnityEvent onRoomExit;
    
    private int playerCount;
    private int currentPlayerNum = 0;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private Vector2 moveDirection;

    private void Start()
    {
        playerCount = GameManager.instance.PlayerCount;
        moveDirection = direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => moveDirection,
        };
    }

    private void OnTriggerEnter(Collider collide)
    {
        var category = collide.gameObject.GetComponent<ObjectCategories>();
        if (category == null) return;
        if (category.Categories.Contains(playerType))
        {
            currentPlayerNum++;
        }
        if (currentPlayerNum < playerCount) return;
        onRoomExit.Invoke();
    }

    public void TransitionRoom()
    {
        CameraManager.MoveCamera(moveDirection, roomCamera);
    }

    public void ExitLevel()
    {
        Debug.Log($"Level Exit Triggered");
        Application.Quit();
    }

    private void OnTriggerExit(Collider collide)
    {
        var category = collide.gameObject.GetComponent<ObjectCategories>();
        if (category == null) return;
        if (category.Categories.Contains(playerType))
        {
            currentPlayerNum--;
        }
    }
}
