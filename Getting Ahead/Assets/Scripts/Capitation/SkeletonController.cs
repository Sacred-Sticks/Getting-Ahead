using System;
using System.Collections;
using Kickstarter.Events;
using Kickstarter.Identification;
using UnityEngine;
using IServiceProvider = Kickstarter.Events.IServiceProvider;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rigidbody))]
public class SkeletonController : MonoBehaviour, IServiceProvider
{
    [SerializeField] private Service onDecapitation;
    [SerializeField] private Service onRecapitation;
    [Space]
    [SerializeField] private HeadStatistics headStatistics;
    [SerializeField] private float headSpeed;
    [Space]
    [SerializeField] private GameObject initialBody;
    [SerializeField] private SkinnedMeshRenderer[] meshes;

    private Player player;
    private Player.PlayerIdentifier playerID;
    private Transform[] initialBones;
    private Rigidbody body;
    private GameObject activeBodyRoot;
    private GameObject skeletonRoot;
    private SkinnedMeshRenderer Skeleton
    {
        set
        {
            var activeBones = initialBones;
            switch (value)
            {
                case null:
                    transform.position = skeletonRoot.transform.parent.position;
                    transform.rotation = skeletonRoot.transform.parent.rotation;
                    break;
                default:
                    activeBones = value.bones;
                    skeletonRoot = value.gameObject;
                    break;
            }

            foreach (var mesh in meshes)
                mesh.bones = activeBones;
        }
    }

    private void OnEnable()
    {
        onDecapitation.Event += ImplementService;
        onRecapitation.Event += ImplementService;
    }

    private void OnDisable()
    {
        onDecapitation.Event -= ImplementService;
        onRecapitation.Event -= ImplementService;
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        body = GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        GetComponent<Movement>().MoveSpeed = headSpeed;
        initialBones = meshes[0].bones;

        var activeRootTransform = initialBody.transform;
        while (activeRootTransform.parent)
            activeRootTransform = activeRootTransform.parent;
        activeBodyRoot = activeRootTransform.gameObject;

        playerID = player.PlayerID;
        Recapitate(initialBody);

        yield return new WaitForSeconds(5);
        
        Decapitate(initialBody);
    }

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case Health.DeathArgs deathArgs:
                Decapitate(deathArgs.DyingCharacterGameObject);
                break;
            case RecapitationArgs recapitationArgs:
                Recapitate(recapitationArgs.ChosenBody);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Decapitate(GameObject dyingBody)
    {
        if (dyingBody != activeBodyRoot)
            return;
        body.useGravity = true;
        Skeleton = null;
        dyingBody.GetComponent<Player>().PlayerID = Player.PlayerIdentifier.None;
        player.PlayerID = playerID;
        // does not yet disable any AI on enemies
        
        switch (playerID)
        {
            case Player.PlayerIdentifier.None:
                WitherBody(dyingBody, 1);
                break;
            default:
                WitherBody(dyingBody, 0.75f);
                break;
        }
    }

    private void Recapitate(GameObject chosenBody)
    {
        body.useGravity = false;
        Skeleton = chosenBody.GetComponentInChildren<SkinnedMeshRenderer>();
        chosenBody.TryGetComponent(out CharacterStatistics characterStatistics);
        characterStatistics.ApplyValues(headStatistics);
        chosenBody.GetComponent<Player>().PlayerID = playerID;
        player.PlayerID = Player.PlayerIdentifier.None;
    }

    private void WitherBody(Object body, float percentage)
    {
        // Rewrite this later to use some visual effect instead of just destroying it
        if (Random.Range(0, 1f) <= percentage)
            Destroy(body);
    }

    public class RecapitationArgs : EventArgs
    {
        public RecapitationArgs(GameObject chosenBody)
        {
            ChosenBody = chosenBody;
        }

        public GameObject ChosenBody { get; }
    }
}
