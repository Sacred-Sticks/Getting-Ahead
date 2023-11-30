using System;
using System.Collections.Generic;
using System.Linq;
using Kickstarter.Events;
using Kickstarter.Identification;
using MyUILibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour, IObserver<Health.DamageTaken>, Kickstarter.Events.IServiceProvider
{
    Player[] playerComponents;
    Player[] players;
    readonly Dictionary<Player.PlayerIdentifier, RadialProgress> UIPairing = new();
    [SerializeField] private Service onRecapitation;

    public void OnNotify(Health.DamageTaken argument)
    {
        Player damagedPlayer = players.Where(p => p.gameObject == argument.Sender).FirstOrDefault();
        var radialHealth = UIPairing[damagedPlayer.PlayerID];
        var maxHealth = argument.MaxHealth;
        var health = argument.Health;
        radialHealth.progress = CalculateHealthPct(maxHealth, health);
    }

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        playerComponents = (Player[])FindObjectsOfType(typeof(Player));
        players = playerComponents.Where(p => p.PlayerID != Player.PlayerIdentifier.None).Where(p => p.GetComponent<HeadPair>() != null).ToArray();

        List<RadialProgress> healthBars = new() { root.Q<RadialProgress>("P1RadialProgress"), root.Q<RadialProgress>("P2RadialProgress"), root.Q<RadialProgress>("P3RadialProgress"), root.Q<RadialProgress>("P4RadialProgress") };

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Health>().AddObserver(this);
            UIPairing.Add(players[i].PlayerID, healthBars[i]);
            healthBars[i].progress = 100;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Health>().RemoveObserver(this);
        }
    }

    private float CalculateHealthPct(float max, float curr)
    {
        float pct = curr / max;
        return 100*pct;
    }

    public void ImplementService(EventArgs args)
    {
        switch (args)
        {
            case SkeletonController.RecapitationArgs recapitationArgs:
                OnRecapitation(recapitationArgs);
                break;
            case Health.DeathArgs deathArgs:
                OnDecapitation(deathArgs);
                break;
        }
    }

    private void OnDecapitation(Health.DeathArgs deathArgs)
    {
        Player damagedPlayer = players.Where(p => p.gameObject == deathArgs.DyingCharacterGameObject).FirstOrDefault();
        UIPairing.Remove(damagedPlayer.PlayerID);
    }

    private void OnRecapitation(SkeletonController.RecapitationArgs args)
    {
        var newBody = args.ChosenBody;
        newBody.GetComponent<Health>().AddObserver(this);

    }
}
