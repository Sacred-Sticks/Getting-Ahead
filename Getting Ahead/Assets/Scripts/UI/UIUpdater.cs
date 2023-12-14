using System;
using Kickstarter.Events;
using Kickstarter.Identification;
using UnityEngine;
using IServiceProvider = Kickstarter.Events.IServiceProvider;

public class UIUpdater : MonoBehaviour, IServiceProvider
{
    [SerializeField] private Service onRecapitation;
    [SerializeField] private Service onDecapitation;
    
    private SkeletonController skeletonController;
    
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        skeletonController = GetComponent<SkeletonController>();
    }

    private void OnEnable()
    {
        onRecapitation.Event += ImplementService;
        onDecapitation.Event += ImplementService;
    }

    private void OnDisable()
    {
        onRecapitation.Event -= ImplementService;
        onDecapitation.Event -= ImplementService;
    }
    
    public void ImplementService(EventArgs args)
    {
        if (player.PlayerID == Player.PlayerIdentifier.None)
            return;
        
        switch (args)
        {
            case SkeletonController.RecapitationArgs recapitationArgs:
                OnRecapitation(recapitationArgs.ChosenBody);
                break;
            case SkeletonController.DecapitationArgs decapitationArgs:
                OnDecapitation(decapitationArgs.DyingBody);
                break;
            default:
                throw new Exception("Incorrect Service Loaded");
        }
    }

    private void OnRecapitation(GameObject chosenBody)
    {
        if (skeletonController.Skeleton == null || chosenBody != skeletonController.Skeleton.transform.root.gameObject)
            return;
        var bodyHealth = chosenBody.GetComponent<Health>();
        HealthUI.instance.ModifyObservedHealth(player.PlayerID, bodyHealth);
        HealthUI.instance.SetPlayerHealthDisplay(player.PlayerID, 100);
    }

    private void OnDecapitation(GameObject deadBody)
    {
        if (skeletonController.Skeleton == null || deadBody != skeletonController.Skeleton.transform.root.gameObject)
            return;
        var bodyHealth = deadBody.GetComponent<Health>();
        HealthUI.instance.SetPlayerHealthDisplay(player.PlayerID, 0);
    }
}