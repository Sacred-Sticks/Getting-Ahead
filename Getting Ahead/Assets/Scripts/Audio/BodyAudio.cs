using System.Collections;
using UnityEngine;
using System.Linq;
using Kickstarter.Events;

[RequireComponent(typeof(AudioSource))]
public class BodyAudio : MonoBehaviour, IObserver<PlayerActions>
{
    [SerializeField] private AudioPairing<PlayerActions>[] audioPairs;
    [SerializeField] private AudioSource walkAudioSource;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioSource damageAudioSource;
    private AudioSource currentAudioSource = null;

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
            return;
        }
        switch (argument)
        {
            case PlayerActions.Moving: currentAudioSource = walkAudioSource; break;
            case PlayerActions.Shooting: currentAudioSource = shootAudioSource; break;
            case PlayerActions.DamageTaken: currentAudioSource = damageAudioSource; break;
            default: currentAudioSource = null; break;
        }
        if (currentAudioSource == null || currentAudioSource.isPlaying) return;
        var audioPair = audioPairs.Where(a => a.State == argument).FirstOrDefault();
        currentAudioSource.clip = audioPair.Clip;
        StartCoroutine(PlayAudio(audioPair.Delay));
    }

    private IEnumerator PlayAudio(float delay)
    {
        while (true)
        {
            currentAudioSource.Play();
            yield return new WaitForSeconds(delay);
        }
    }
}
