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

    // Start is called before the first frame update
    void Start()
    {
        switch (direction)
        {
            case Direction.LEFT: moveDirection = Vector2.left; break;
            case Direction.RIGHT: moveDirection = Vector2.right; break;
            case Direction.UP: moveDirection = Vector2.up; break;
            case Direction.DOWN: moveDirection = Vector2.down; break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var cat = collision.gameObject.GetComponent<ObjectCategories>();
        if (cat.Categories.Contains(playerType)){
            Debug.Log("TRANSITIONING!");
            onRoomChange.Trigger(new CameraScript.RoomChangeArgs(moveDirection));
        }
    }
}
