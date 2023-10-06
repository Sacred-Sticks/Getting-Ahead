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
        CameraInput.SubscribeToInputAction(ReceiveInput, Player.PlayerIdentifier.KeyboardAndMouse);
        vcam2.m_Priority = 1;
        vcam1.m_Priority = 2;
    }

    void ReceiveInput(float input)
    {
        switch (input)
        {
            case 1:
                vcam2.m_Priority = 0;
                vcam1.m_Priority = 9;
                break;
            case -1:
                vcam1.m_Priority = 0;
                vcam2.m_Priority = 9;
                break;
        }
    }
}

