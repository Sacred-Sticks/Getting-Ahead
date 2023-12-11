using System.Collections;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour, IObserver<PlayerActions>
{
    [SerializeField] private AudioPairing<PlayerActions>[] audioPairs;
    private AudioSource audioSource;
    private bool audioPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GetComponent<Movement>().AddObserver(this);
        GetComponent<Attack>().AddObserver(this);
        GetComponent<Health>().AddObserver(this);
    }

    private void OnDisable()
    {
        GetComponent<Movement>().RemoveObserver(this);
        GetComponent<Attack>().RemoveObserver(this);
        GetComponent<Health>().RemoveObserver(this);
    }

    public void OnNotify(PlayerActions argument)
    {
        if (argument == PlayerActions.STOP)
        {
            StopAllCoroutines();
            audioPlaying = false;
            return;
        }
            if (audioPlaying)
            return;
        var audioPair = audioPairs.Where(a => a.State == argument).FirstOrDefault();
        audioSource.clip = audioPair.Clip;
        StartCoroutine(PlayAudio(audioPair.Delay));
    }

    private IEnumerator PlayAudio(float delay)
    {
        audioPlaying = true;
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(delay);
        }
    }
}
