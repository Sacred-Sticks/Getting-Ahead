using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Rotation : MonoBehaviour
{
    [SerializeField]
    private Vector2Input rotationInput;
    [SerializeField]
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        rotationInput.SubscribeToInputAction(RecieveInput, player.PlayerID);
    }

    private void RecieveInput(Vector2 input)
    {  
        Debug.Log("Input recieved!!!");
        float angleA = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angleA, 0f);
        Debug.Log("Angle is: " +angleA);

    }

}
