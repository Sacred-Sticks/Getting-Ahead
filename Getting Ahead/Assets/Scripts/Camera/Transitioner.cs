using System.Collections;
using System.Collections.Generic;
using Kickstarter.Events;
using Kickstarter.Categorization;
using UnityEngine;
using System.Linq;
using System;

public class Transitioner : MonoBehaviour
{
    enum Direction { LEFT, RIGHT, UP, DOWN };
    [SerializeField] Direction direction;
    [SerializeField] Service onRoomChange;
    [SerializeField] CategoryType playerType;
    Vector2 moveDirection;
    Vector2 positionOffset;
    [SerializeField] float offsetAmount = 1;

    void Start()
    {
        switch (direction)
        {
            case Direction.LEFT: 
                moveDirection = Vector2.left;
                break;
            case Direction.RIGHT: 
                moveDirection = Vector2.right;
                break;
            case Direction.UP: 
                moveDirection = Vector2.up; 
                break;
            case Direction.DOWN: 
                moveDirection = Vector2.down; 
                break;
        }
        positionOffset = moveDirection * offsetAmount;
    }

    private void OnTriggerExit(Collider collide)
    {
        switch (direction)
        {
            case Direction.LEFT:
                if (collide.gameObject.transform.position.x > transform.position.x + positionOffset.x) return;
                break;
            case Direction.RIGHT:
                if (collide.gameObject.transform.position.x < transform.position.x + positionOffset.x) return;
                break;
            case Direction.UP:
                if (collide.gameObject.transform.position.z < transform.position.z + positionOffset.y) return;
                break;
            case Direction.DOWN:
                if (collide.gameObject.transform.position.z > transform.position.z + positionOffset.y) return;
                break;

        }
        var cat = collide.gameObject.GetComponent<ObjectCategories>();
        
        if (cat.Categories.Contains(playerType)){
            Debug.Log("TRANSITIONING!");
            onRoomChange.Trigger(new CameraScript.RoomChangeArgs(moveDirection));
        }
    }
}
