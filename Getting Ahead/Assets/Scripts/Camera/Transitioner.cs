using System.Collections;
using System.Collections.Generic;
using Kickstarter.Events;
using Kickstarter.Categorization;
using UnityEngine;
using System.Linq;
using System;

public class Transitioner : MonoBehaviour
{
    [SerializeField] private Service onRoomChange;
    [SerializeField] private CategoryType playerType;
    [SerializeField] private float offsetAmount = 1;


    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    
    private Vector2 moveDirection;
    private Vector2 positionOffset;
    private Direction direction;

    public void Initialize(Direction direction)
    {
        this.direction = direction;
        moveDirection = direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => moveDirection,
        };
        positionOffset = moveDirection * offsetAmount;
    }

    private void OnTriggerExit(Collider collide)
    {
        switch (direction)
        {
            case Direction.Up:
                if (collide.gameObject.transform.position.z < transform.position.z + positionOffset.y) return;
                break;
            case Direction.Down:
                if (collide.gameObject.transform.position.z > transform.position.z + positionOffset.y) return;
                break;
            case Direction.Left:
                if (collide.gameObject.transform.position.x > transform.position.x + positionOffset.x) return;
                break;
            case Direction.Right:
                if (collide.gameObject.transform.position.x < transform.position.x + positionOffset.x) return;
                break;
        }
        var cat = collide.gameObject.GetComponent<ObjectCategories>();
        if (cat == null) return;
        if (cat.Categories.Contains(playerType)){
            onRoomChange.Trigger(new CameraManager.RoomChangeArgs(moveDirection));
        }
    }
}
