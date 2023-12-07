using UnityEngine;

public class RoomBlocker : MonoBehaviour
{
    [SerializeField] private GameObject blockers;

    public static RoomBlocker[] RoomBlockers;

    private static int roomNumber;

    private void Awake()
    {
        RoomBlockers = FindObjectsOfType<RoomBlocker>();
    }

    private void Start()
    {
        if (roomNumber > 0)
            blockers.SetActive(false);
        roomNumber++;
    }

    public void ActivateBlockers()
    {
        if (!CameraManager.IsRoomEmpty())
            return;
        ToggleBlockers();
    }

    public void ToggleBlockers()
    {
        foreach (var roomBlocker in RoomBlockers)
        {
            roomBlocker.DisableBlocks();
        }
        blockers.SetActive(true);
    }

    public void DisableBlocks()
    {
        blockers.SetActive(false);
    }
}
