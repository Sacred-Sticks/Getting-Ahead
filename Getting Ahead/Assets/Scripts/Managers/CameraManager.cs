using System.Collections.Generic;
using Cinemachine;
using Kickstarter.Events;
using Kickstarter.Observer;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Service onRoomChange;
    private static CinemachineVirtualCamera[] CameraObjects;
    private static readonly Dictionary<Vector3, CinemachineVirtualCamera> virtualCameras = new Dictionary<Vector3, CinemachineVirtualCamera>();
    private static CinemachineVirtualCamera currentCamera;

    public static void SetupCameraDictionary(GameObject initialRoom)
    {
        currentCamera = initialRoom.GetComponentInChildren<CinemachineVirtualCamera>();

        CameraObjects = FindObjectsOfType<CinemachineVirtualCamera>();
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

    private static void SwapCamera(CinemachineVirtualCamera newCam)
    {
        currentCamera.m_Priority = 0;
        newCam.m_Priority = 1;
        currentCamera = newCam;
    }

    public static void MoveCamera(Vector2 input, CinemachineVirtualCamera roomCamera)
    {
        if (roomCamera == currentCamera) return;
        var tempVector3 = currentCamera.transform.position;
        switch (input.x)
        {
            case 1:
                { // go left
                    tempVector3.x -= 15;
                    break;
                }
            case -1: // go right
                {
                    tempVector3.x += 15;
                    break;
                }
        }
        switch (input.y)
        {
            case 1:
                { // go up
                    tempVector3.z -= 15;
                    break;
                }
            case -1: // go down
                {
                    tempVector3.z += 15;
                    break;
                }
        }
        if (virtualCameras.ContainsKey(tempVector3))
        {
            SwapCamera(virtualCameras[tempVector3]);
        };
    }

}


