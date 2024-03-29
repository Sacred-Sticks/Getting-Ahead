using System;
using System.Collections;
using System.Linq;
using Kickstarter.Events;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;
using IServiceProvider = Kickstarter.Events.IServiceProvider;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rigidbody))]
public class SkeletonController : Observable, IInputReceiver, IServiceProvider
{
    [SerializeField] private FloatInput recapitateInput;
    [SerializeField] private FloatInput decapitateInput;
    [Space]
    [SerializeField] private Service onDeath;
    [Space]
    [SerializeField] private Service onDecapitation;
    [SerializeField] private Service onRecapitation;
    [Space]
    [SerializeField] private HeadStatistics headStatistics;
    [SerializeField] private float headSpeed;
    [Space]
    [SerializeField] private Transform headBone;
    [SerializeField] private Transform colliderTransform;

    private const float recapitationRange = 1;

    private Player player;
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
                    colliderTransform.position = activeBodyRoot.GetComponent<HeadPair>().HeadRoot.position;
                    StopCoroutine(copyPositionRoutine);
                    copyPositionRoutine = null;
                    activeBodyRoot = null;
                    break;
                default:
                    value.transform.parent.parent.GetComponent<HeadPair>().Head = gameObject;
                    activeBones = value.bones;
                    skeletonRoot = value.gameObject;

                    var root = value.transform;
                    while (root.parent)
                        root = root.parent;
                    activeBodyRoot = root.gameObject;
                    transform.GetChild(0).position = activeBodyRoot.GetComponent<HeadPair>().HeadRoot.position;
                    copyPositionRoutine = StartCoroutine(CopyPosition());
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

    #region Unity Events
    private void OnEnable()
    {
        onDeath.Event += ImplementService;
    }

    private void OnDisable()
    {
        onDeath.Event -= ImplementService;
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
    #endregion

    #region Inputs
    public void SubscribeToInputs(Player player)
    {
        recapitateInput.SubscribeToInputAction(OnRecapitateInputChange, player.PlayerID);
        decapitateInput.SubscribeToInputAction(OnDecapitateInputChange, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        recapitateInput.UnsubscribeToInputAction(OnRecapitateInputChange, player.PlayerID);
        decapitateInput.UnsubscribeToInputAction(OnDecapitateInputChange, player.PlayerID);
    }

    private void OnRecapitateInputChange(float input)
    {
        if (input == 0)
            return;
        if (activeBodyRoot)
            return;
        var overlappingObjects = Physics.OverlapSphere(transform.position + Vector3.up * 2, recapitationRange);
        var selectedBody = overlappingObjects
            .Where(o => o.GetComponentInChildren<HeadPair>())
            .Select(o => o.gameObject)
            .FirstOrDefault();
        if (selectedBody)
            Recapitate(selectedBody);
    }

    private void OnDecapitateInputChange(float input)
    {
        if (input == 0)
            return;
        Decapitate(activeBodyRoot);
    }
    #endregion

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case Health.DeathArgs deathArgs:
                Decapitate(deathArgs.DyingCharacterGameObject);
                break;
            default:
                throw new Exception("Invalid Service Used");
        }
    }

    public void Decapitate(GameObject dyingBody)
    {
        if (!activeBodyRoot)
            return;
        if (dyingBody != activeBodyRoot)
            return;
        body.useGravity = true;
        onDecapitation.Trigger(new DecapitationArgs(dyingBody));
        Skeleton = null;
        NotifyObservers<GameObject>(null);
        NotifyObservers<PlayerActions>(PlayerActions.Decap);
        NotifyObservers<PlayerActions>(PlayerActions.STOP);
        var dyingPlayer = dyingBody.GetComponent<Player>();
        player.PlayerID = dyingPlayer.PlayerID;
        dyingPlayer.PlayerID = Player.PlayerIdentifier.None;
        // does not yet disable any AI on enemies

        
        const float witherPlayer = 1;
        const float witherEnemy = 0.75f;

        switch (player.PlayerID)
        {
            case Player.PlayerIdentifier.None:
                WitherBody(dyingBody, witherEnemy);
                break;
            default:
                WitherBody(dyingBody, witherPlayer);
                break;
        }
    }

    public void Recapitate(GameObject chosenBody)
    {
        var headPair = chosenBody.GetComponent<HeadPair>();
        if (headPair.Head)
            return;
        body.useGravity = false;
        Skeleton = chosenBody.GetComponentInChildren<SkinnedMeshRenderer>();
        NotifyObservers(chosenBody);
        chosenBody.TryGetComponent(out CharacterStatistics characterStatistics);
        characterStatistics.ApplyValues(headStatistics);
        chosenBody.GetComponent<Player>().PlayerID = player.PlayerID;
        chosenBody.TryGetComponent(out Movement movement);
        if (player.PlayerID != Player.PlayerIdentifier.None)
            movement.enabled = true;
        onRecapitation.Trigger(new RecapitationArgs(gameObject, chosenBody));
        NotifyObservers<PlayerActions>(PlayerActions.Recap);
        NotifyObservers<PlayerActions>(PlayerActions.STOP);
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
            if (skeletonRoot == null)
                yield break;
            transform.position = skeletonRoot.transform.parent.parent.position;
            transform.rotation = skeletonRoot.transform.parent.parent.rotation;
            yield return new WaitForEndOfFrame();
        }
    }

    public class RecapitationArgs : EventArgs
    {
        public RecapitationArgs(GameObject sender, GameObject chosenBody)
        {
            Sender = sender;
            ChosenBody = chosenBody;
        }

        public GameObject ChosenBody { get; }
        public GameObject Sender { get; }
    }

    public class DecapitationArgs : EventArgs
    {
        public DecapitationArgs(GameObject dyingbody)
        {
            DyingBody = dyingbody;
        }
        
        public GameObject DyingBody { get; }
    }
}
