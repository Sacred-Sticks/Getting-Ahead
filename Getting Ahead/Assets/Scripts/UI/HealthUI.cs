using System;
using System.Collections.Generic;
using System.Linq;
using Kickstarter.Events;
using Kickstarter.Identification;
using MyUILibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour, IObserver<Health.DamageTaken>
{
    [SerializeField] private Service onRecapitation;

    public static HealthUI instance { get; private set; }

    private Player[] playerComponents;
    private Player[] players;
    private readonly Dictionary<Player.PlayerIdentifier, RadialProgress> playerRadials = new Dictionary<Player.PlayerIdentifier, RadialProgress>();
    private readonly Dictionary<Player.PlayerIdentifier, Health> playerHealths = new Dictionary<Player.PlayerIdentifier, Health>();

    private VisualElement root;

    private void Awake()
    {
        instance = this;
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void Start()
    {
        playerComponents = (Player[])FindObjectsOfType(typeof(Player));
        players = playerComponents.Where(p => p.PlayerID != Player.PlayerIdentifier.None).Where(p => p.GetComponent<HeadPair>() != null).ToArray();

        var healthBars = new List<RadialProgress>
        {
            root.Q<RadialProgress>("P1RadialProgress"),
            root.Q<RadialProgress>("P2RadialProgress"),
            root.Q<RadialProgress>("P3RadialProgress"),
            root.Q<RadialProgress>("P4RadialProgress"),
        };

        for (int i = 0; i < players.Length; i++)
        {
            var health = players[i].GetComponent<Health>();
            health.AddObserver(this);
            playerRadials.Add(players[i].PlayerID, healthBars[i]);
            playerHealths.Add(players[i].PlayerID, health);
            healthBars[i].progress = 100;
        }

        for (int i = 0; i < 4 - players.Length; i++)
        {
            RemoveRadialProgress(healthBars[3 - i], healthBars);
        }
    }

    private void RemoveRadialProgress(RadialProgress element, ICollection<RadialProgress> radials)
    {
        var parent = element.hierarchy.parent;
        parent.hierarchy.Remove(element);
        radials.Remove(element);
    }

    public void OnNotify(Health.DamageTaken argument)
    {
        var damagedPlayer = argument.Sender.GetComponent<Player>();
        if (damagedPlayer.PlayerID == Player.PlayerIdentifier.None)
            return;
        SetPlayerHealthDisplay(damagedPlayer.PlayerID, CalculateHealthPct(argument.MaxHealth, argument.Health));
    }

    private float CalculateHealthPct(float max, float curr)
    {
        float pct = curr / max;
        return 100 * pct;
    }

    public void SetPlayerHealthDisplay(Player.PlayerIdentifier playerID, float healthPercentage)
    {
        if (!playerRadials.ContainsKey(playerID))
            return;
        var radialHealth = playerRadials[playerID];
        radialHealth.progress = healthPercentage;
    }

    public void ModifyObservedHealth(Player.PlayerIdentifier playerID, Health health)
    {
        if (!playerHealths.ContainsKey(playerID))
            return;
        playerHealths[playerID].RemoveObserver(this);
        playerHealths[playerID] = health;
        playerHealths[playerID].AddObserver(this);
    }
}
