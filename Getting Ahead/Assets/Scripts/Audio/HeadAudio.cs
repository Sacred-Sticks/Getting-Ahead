using System.Collections;
using UnityEngine;
using System.Linq;
using Kickstarter.Events;

[RequireComponent(typeof(AudioSource))]
public class HeadAudio : MonoBehaviour, IObserver<PlayerActions>
{
    [SerializeField] private AudioPairing<PlayerActions>[] audioPairs;
    [SerializeField] private AudioSource recapAudioSource;
    [SerializeField] private AudioSource decapAudioSource;
    private AudioSource currentAudioSource = null;

    private void OnEnable()
    {
        GetComponent<SkeletonController>().AddObserver(this);
    }

    private void OnDisable()
    {
        GetComponent<SkeletonController>().RemoveObserver(this);
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
            case PlayerActions.Recap: currentAudioSource = recapAudioSource; break;
            case PlayerActions.Decap: currentAudioSource = decapAudioSource; break;
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
