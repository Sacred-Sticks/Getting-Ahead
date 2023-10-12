using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kickstarter.Inputs;
using UnityEngine;
using Kickstarter.Identification;
using System.Linq;

public class CameraScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;
    [SerializeField] FloatInput CameraInput;
    private GameObject[] CameraObjects;
    void Start()
    {
        CameraInput.SubscribeToInputAction(ReceiveInput, Player.PlayerIdentifier.KeyboardAndMouse);
        vcam2.m_Priority = 1;
        vcam1.m_Priority = 2;
        CameraObjects = GameObject.FindGameObjectsWithTag("VirtualCamera");
    }

    void ReceiveInput(float input)
    {
        switch (input)
        {
            case 1:
                CameraObjects[0].GetComponent<CinemachineVirtualCamera>().m_Priority = 9;
                CameraObjects[1].GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                break;
            case -1:
                CameraObjects[1].GetComponent<CinemachineVirtualCamera>().m_Priority = 9;
                CameraObjects[0].GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                break;
        }
    }
}

