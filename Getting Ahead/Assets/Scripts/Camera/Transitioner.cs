using Kickstarter.Events;
using Kickstarter.Categorization;
using UnityEngine;
using System.Linq;
using System;
using Cinemachine;

public class Transitioner : MonoBehaviour
{
    [SerializeField] Direction direction;
    [SerializeField] private CategoryType playerType;
    [SerializeField] private CinemachineVirtualCamera roomCamera;
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
        NewMethod(collide);
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

    //private void OldMethod(Collider collide)
    //{
    //    switch (direction)
    //    {
    //        case Direction.Up:
    //            if (collide.gameObject.transform.position.z < transform.position.z + positionOffset.y) return;
    //            break;
    //        case Direction.Down:
    //            if (collide.gameObject.transform.position.z > transform.position.z + positionOffset.y) return;
    //            break;
    //        case Direction.Left:
    //            if (collide.gameObject.transform.position.x > transform.position.x + positionOffset.x) return;
    //            break;
    //        case Direction.Right:
    //            if (collide.gameObject.transform.position.x < transform.position.x + positionOffset.x) return;
    //            break;
    //    }
    //    var cat = collide.gameObject.GetComponent<ObjectCategories>();
    //    if (cat == null) return;
    //    if (cat.Categories.Contains(playerType))
    //    {
    //        onRoomChange.Trigger(new CameraManager.RoomChangeArgs(moveDirection));
    //    }
    //}
    private void NewMethod(Collider collide)
    {

        CameraManager.MoveCamera(moveDirection, roomCamera);
    }
}
