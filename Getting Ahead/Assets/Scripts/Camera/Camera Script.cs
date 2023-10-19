using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kickstarter.Inputs;
using UnityEngine;
using Kickstarter.Identification;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using Kickstarter.Events;
using System;

public class CameraScript : MonoBehaviour, Kickstarter.Events.IServiceProvider
{
    [SerializeField] Service onRoomChange;
    private CinemachineVirtualCamera[] CameraObjects;
    private readonly Dictionary<Vector3, CinemachineVirtualCamera> virtualCameras = new Dictionary<Vector3, CinemachineVirtualCamera>();
    [SerializeField] private CinemachineVirtualCamera currentCamera;


    private void OnEnable()
    {
        onRoomChange.Event += ImplementService;
    }

    private void OnDisable()
    {
        onRoomChange.Event -= ImplementService;
    }
    public void SetupCameraDictionary()
    {
        CameraObjects = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var obj in CameraObjects)
        {
            virtualCameras.Add(obj.gameObject.transform.position, obj);
        }
        foreach (var cam in virtualCameras)
        {
            cam.Value.m_Priority = 0;
        }
        currentCamera.m_Priority = 9;
    }
    void SwapCamera(CinemachineVirtualCamera newCam)
    {
        currentCamera.m_Priority = 0;
        newCam.m_Priority = 9;
        currentCamera = newCam;
    }
    void MoveCamera(Vector2 input)
    {
        Debug.Log("TRIGGERING!");
        Vector3 tempVector3 = currentCamera.transform.position;
        switch (input.x)
        {
            case 1:
                { // go left
                    tempVector3.x += 15;
                    break;
                }
            case -1: // go right
                {
                    tempVector3.x -= 15;
                    break;
                }
        }
        switch (input.y)
        {
            case 1:
                { // go up
                    tempVector3.z += 15;
                    break;
                }
            case -1: // go down
                {
                    tempVector3.z -= 15;
                    break;
                }
        }
        if (virtualCameras.ContainsKey(tempVector3))
        {
            SwapCamera(virtualCameras[tempVector3]);
        };
    }

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case RoomChangeArgs roomChangeArgs:
                MoveCamera(roomChangeArgs.RoomPosition);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
        
    }
    public class RoomChangeArgs: EventArgs
    {
        public Vector2 RoomPosition { get; }
        public RoomChangeArgs(Vector2 roomPosition)
        {
            RoomPosition = roomPosition;
        }
    }
}


