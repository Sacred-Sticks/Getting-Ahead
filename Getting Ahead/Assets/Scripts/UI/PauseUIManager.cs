using System;
using Kickstarter.Events;
using UnityEngine;
using UnityEngine.UIElements;
using IServiceProvider = Kickstarter.Events.IServiceProvider;

public class PauseUIManager : MonoBehaviour, IServiceProvider
{
    [SerializeField] private Service onTriggerPause;

    private UIDocument PauseUI;
    private bool paused;
    private int inputCount;

    private void OnEnable() => onTriggerPause.Event += ImplementService;
    private void OnDisable() => onTriggerPause.Event -= ImplementService;

    private void Awake()
    {
        PauseUI = GetComponent<UIDocument>();
    }

    private void Start()
    {
        PauseUI.enabled = false;
        paused = false;
    }

    public void PauseGame()
    {
        paused = true;
        PauseUI.enabled = true;
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        paused = false;
        PauseUI.enabled = false;
        Time.timeScale = 1;
    }

    private void TogglePause(float input)
    {
        if (input == 0) return;
        if (!paused) PauseGame();
        else UnPauseGame();
    }

    public void ImplementService(EventArgs args)
    {
        // Not proud of this, turns out my inputs register twice for some reason, and then double that (two for the head and two for the body).
        // Game jam solution because I don't have time to look for and solve the problem so instead we're doing this because it is functional.
        inputCount++;
        if (inputCount % 4 != 0)
            return;
        switch (args)
        {
            case PauseTrigger.OnPauseTrigger onPauseTrigger:
                TogglePause(onPauseTrigger.PauseInput);
                break;
            default:
                throw new Exception("Invalid Service Implemented");
        }
    }
}