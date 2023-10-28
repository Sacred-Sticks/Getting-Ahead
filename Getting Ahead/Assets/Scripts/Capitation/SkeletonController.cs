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

    private Player player;
    private Player.PlayerIdentifier playerID;
    private SkinnedMeshRenderer[] meshes;
    private Transform[] initialBones;
    private Rigidbody body;
    private GameObject activeBodyRoot;
    private GameObject skeletonRoot;
    private Coroutine copyPositionRoutine;
    private SkinnedMeshRenderer skeleton;
    public SkinnedMeshRenderer Skeleton
    {
        private set
        {
            skeleton = value;
            var activeBones = initialBones;
            switch (value)
            {
                case null:
                    if (copyPositionRoutine == null)
                        break;
                    StopCoroutine(copyPositionRoutine);
                    copyPositionRoutine = null;
                    break;
                default:
                    value.transform.parent.parent.GetComponent<HeadPair>().Head = gameObject;
                    activeBones = value.bones;
                    skeletonRoot = value.gameObject;

                    var root = value.transform;
                    while (root.parent)
                        root = root.parent;
                    activeBodyRoot = root.gameObject;
                    StartCoroutine(CopyPosition());
                    break;
            }

            foreach (var mesh in meshes)
                mesh.bones = activeBones;
        }
        get
        {
            return skeleton;
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

        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        GetComponent<Movement>().MoveSpeed = headSpeed;
        initialBones = meshes[0].bones;
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

    public void Decapitate(GameObject dyingBody)
    {
        if (dyingBody.name != activeBodyRoot.name)
            return;
        body.useGravity = true;
        Skeleton = null;
        var dyingPlayer = dyingBody.GetComponent<Player>();
        player.PlayerID = dyingPlayer.PlayerID;
        dyingPlayer.PlayerID = Player.PlayerIdentifier.None;
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

    public void Recapitate(GameObject chosenBody)
    {
        body.useGravity = false;
        Skeleton = chosenBody.GetComponentInChildren<SkinnedMeshRenderer>();
        chosenBody.TryGetComponent(out CharacterStatistics characterStatistics);
        characterStatistics.ApplyValues(headStatistics);
        chosenBody.GetComponent<Player>().PlayerID = player.PlayerID;
        player.PlayerID = Player.PlayerIdentifier.None;
    }

    private void WitherBody(Object body, float percentage)
    {
        // Rewrite this later to use some visual effect instead of just destroying it
        if (Random.Range(0, 1f) <= percentage)
            Destroy(body);
    }

    private IEnumerator CopyPosition()
    {
        while (true)
        {
            transform.position = skeletonRoot.transform.parent.parent.position;
            transform.rotation = skeletonRoot.transform.parent.parent.rotation;
            yield return new WaitForEndOfFrame();
        }
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
