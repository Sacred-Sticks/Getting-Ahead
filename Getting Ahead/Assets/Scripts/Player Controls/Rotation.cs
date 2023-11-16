using System.Collections;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Rotation : MonoBehaviour, IInputReceiver
{
    [SerializeField] private Vector2Input movementInput;
    [SerializeField] private Vector2Input rotationInput;
    [SerializeField] [Range(0, 1)] private float deadzone;
    [SerializeField] private float slerpRate;

    private Vector2 rawRotationInput;
    private Vector2 rawMovementInput;

    #region Unity Events
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
    }

    private void Update()
    {
        RotatePlayer(rawRotationInput.sqrMagnitude > deadzone * deadzone ? rawRotationInput : rawMovementInput);
    }
    #endregion

    #region Inputs
    public void SubscribeToInputs(Player player)
    {
        movementInput.SubscribeToInputAction(OnMovementInputChange, player.PlayerID);
        rotationInput.SubscribeToInputAction(OnRotationInputChange, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        movementInput.UnsubscribeToInputAction(OnMovementInputChange, player.PlayerID);
        rotationInput.UnsubscribeToInputAction(OnRotationInputChange, player.PlayerID);
    }

    private void OnMovementInputChange(Vector2 input)
    {
        rawMovementInput = input;
    }

    private void OnRotationInputChange(Vector2 input)
    {
        rawRotationInput = input;
    }
    #endregion

    private void RotatePlayer(Vector2 input)
    {
        if (input.sqrMagnitude < deadzone * deadzone)
            return;
        float desiredAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, desiredAngle, 0), slerpRate);
    }
}
