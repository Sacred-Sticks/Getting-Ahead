using System.Collections;
using Kickstarter.Identification;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(SkeletonController))]
public class BodyCountTracker : MonoBehaviour, IObserver<GameObject>
{
    [SerializeField] private float gameLoseTimer = 5;
    
    private static int count;
    
    private void Start()
    {
        if (GetComponent<Player>().PlayerID != Player.PlayerIdentifier.None)
            GetComponent<SkeletonController>().AddObserver(this);
    }

    public void OnNotify(GameObject argument)
    {
        UpdateCount(argument);
    }

    private void UpdateCount(GameObject argument)
    {
        if (!argument)
            count++;
        else count--;
        if (count == GameManager.instance.PlayerCount)
            StartCoroutine(EndGameTimer());
        else StopAllCoroutines();
    }

    private IEnumerator EndGameTimer()
    {
        yield return new WaitForSeconds(gameLoseTimer);
        GameManager.instance.ChangeScene("GameOver");
    }
}