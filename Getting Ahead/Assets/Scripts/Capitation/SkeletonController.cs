using System;
using Kickstarter.Events;
using Kickstarter.Identification;
using UnityEngine;
using IServiceProvider = Kickstarter.Events.IServiceProvider;

[RequireComponent(typeof(Player))]
public class SkeletonController : MonoBehaviour, IServiceProvider
{
    [SerializeField] private Service onDecapitation;
    [SerializeField] private Service onRecapitation;
    [Space]
    [SerializeField] private GameObject initialBody;
    [SerializeField] private SkinnedMeshRenderer[] meshes;

    private Transform[] initialBones;
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
    private Player player;
    private Player.PlayerIdentifier playerID;
    private GameObject activeBodyRoot;

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
    }

    private void Start()
    {
        initialBones = meshes[0].bones;
        Recapitate(initialBody);
        
        var activeRootTransform = initialBody.transform;
        while (activeRootTransform.parent)
            activeRootTransform = activeRootTransform.parent;
        activeBodyRoot = activeRootTransform.gameObject;
        
        playerID = activeBodyRoot.GetComponent<Player>().PlayerID;
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
        Skeleton = null;
        // Inputs currently assume it to be a player that died, this will throw errors on AI
        dyingBody.GetComponent<Player>().PlayerID = Player.PlayerIdentifier.None;
        player.PlayerID = playerID;
    }

    private void Recapitate(GameObject chosenBody)
    {
        Skeleton = chosenBody.GetComponentInChildren<SkinnedMeshRenderer>();
        // Inputs currently assume it to be a player that died, this will throw errors on AI
        chosenBody.GetComponent<Player>().PlayerID = playerID;
        player.PlayerID = Player.PlayerIdentifier.None;
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
