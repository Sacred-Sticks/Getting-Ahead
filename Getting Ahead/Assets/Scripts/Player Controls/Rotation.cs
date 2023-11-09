using System;
using System.Collections;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Rotation : MonoBehaviour, IInputReceiver
{
    [SerializeField]
    private Vector2Input rotationInput;
    [SerializeField]
    [Range(0, 1)]
    private float deadzone;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
    }

    public void SubscribeToInputs(Player player)
    {
        rotationInput.SubscribeToInputAction(RecieveInput, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        rotationInput.UnsubscribeToInputAction(RecieveInput, player.PlayerID);
    }

    private void RecieveInput(Vector2 input)
    {
        if (input.sqrMagnitude < deadzone * deadzone) // Squared for performance sake
            return;
        float angleA = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angleA, 0f);
    }
}
