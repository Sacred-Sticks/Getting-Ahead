using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kickstarter.Inputs;
using UnityEngine;
using Kickstarter.Identification;

public class CameraScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;
    [SerializeField] FloatInput CameraInput;
    void Start()
    {
        CameraInput.SubscribeToInputAction(RecieveInput, Player.PlayerIdentifier.KeyboardAndMouse);
        vcam2.m_Priority = 2;
        vcam1.m_Priority = 1;
    }

    void RecieveInput(float input)
    {
        Debug.Log("INPUT!");
        if (input == 1)
        { 
            vcam2.m_Priority = 0;
            vcam1.m_Priority = 9;
        }
        else if (input == -1)
        {
            vcam1.m_Priority = 0;
            vcam2.m_Priority = 9;
        }
    }
}

