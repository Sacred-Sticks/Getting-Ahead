using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kickstarter.Inputs;
using UnityEngine;
using Kickstarter.Identification;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class CameraScript : MonoBehaviour
{
    [SerializeField] FloatInput CameraInput;
    private CinemachineVirtualCamera[] CameraObjects;
    private readonly Dictionary<Vector3, CinemachineVirtualCamera> virtualCameras = new Dictionary<Vector3, CinemachineVirtualCamera>();
    [SerializeField] private CinemachineVirtualCamera currentCamera;
    void Start()
    {
        CameraInput.SubscribeToInputAction(ReceiveInput, Player.PlayerIdentifier.KeyboardAndMouse);
        CameraObjects = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        foreach(var obj in CameraObjects)
        {
            virtualCameras.Add(obj.gameObject.transform.position, obj);
        }
        foreach (var cam in virtualCameras)
        {
            cam.Value.m_Priority = 0;
        }
        currentCamera.m_Priority = 9;
    }

    void swapCamera(CinemachineVirtualCamera newCam)
    {
        currentCamera.m_Priority = 0;
        newCam.m_Priority = 9;
        currentCamera = newCam;
    }
    void ReceiveInput(float input)
    {
        float mindist = Mathf.Infinity;
        CinemachineVirtualCamera minCamera = null;
        Vector3 directionMove = Vector3.zero;
        switch (input)
        {
            case 1: // User moves to left
                directionMove = Vector3.left;                
                break;
            case -1:// User moves to right
                directionMove = Vector3.right;
                break;
        }

        foreach (var dictCamera in virtualCameras)
        {
            if (dictCamera.Value.Equals(currentCamera)) continue; // skip currentcamera

            float directionDifference = Vector3.Dot(directionMove, dictCamera.Key - currentCamera.transform.position);//Get distance between camera being checked and current camera in a single direction

            if (directionDifference > 0)
            {
                Debug.Log("Found a camera!");
                float distance = Vector3.Distance(dictCamera.Key, currentCamera.transform.position);
                if (distance < mindist) //If camera being checked is closer than the one which is the closest so far
                {
                    Debug.Log("Found a closer camera! " + dictCamera.Value.name);
                    minCamera = dictCamera.Value; //Set a new minimum distance camera to check against
                    mindist = distance; //Set a new minimum distance to check
                }
            }

        }
        if (minCamera != null) swapCamera(minCamera);

    }
}

