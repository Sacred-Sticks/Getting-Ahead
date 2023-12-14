using System.Collections.Generic;
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
    private readonly List<GameObject> playerObjects = new List<GameObject>();
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
        var parent = collide.transform.parent;
        if (!parent)
            return;
        var category = parent.GetComponent<ObjectCategories>();
        if (category == null)
            return; 
        if (category.Categories.Contains(playerType))
        {
            if (!playerObjects.Contains(parent.gameObject))
                playerObjects.Add(parent.gameObject);
        }
        if (playerObjects.Count < playerCount) return;
        onRoomExit.Invoke();
    }

    private void OnTriggerExit(Collider collide)
    {
        var parent = collide.transform.parent;
        if (!parent)
            return;
        var category = parent.GetComponent<ObjectCategories>();
        if (category == null) return;
        if (!category.Categories.Contains(playerType))
            return;
        if (playerObjects.Contains(parent.gameObject))
            playerObjects.Remove(parent.gameObject);
    }

    public void TransitionRoom()
    {
        CameraManager.MoveCamera(moveDirection, roomCamera);
    }

    public void ExitLevel()
    {
        GameManager.instance.ChangeScene("GoodEnding");
    }
}
