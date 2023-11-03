using UnityEngine;

public class HeadPair : MonoBehaviour
{
    public GameObject Head { get; set; }
    [SerializeField] private Transform headRoot;
    
    public Transform HeadRoot {
        get
        {
            return headRoot;
        }
    }
}
