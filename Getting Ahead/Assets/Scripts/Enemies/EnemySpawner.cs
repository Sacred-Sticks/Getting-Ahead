﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Kickstarter.Events;
using Kickstarter.Identification;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class EnemySpawner : MonoBehaviour, IObserver<Health.DamageTaken>
{
    [SerializeField] private Service onRoomChange;
    [SerializeField] private EnemyDetails[] enemies;
    [SerializeField] private BoxCollider spawnRange;
    [SerializeField] private int enemyPoints;
    
    private CinemachineVirtualCamera virtualCamera;
    private CameraManager cameraManager;

    public int EnemyCount { get; private set; }
    
    #region Unity Events
    private void OnEnable()
    {
        cameraManager = GameManager.instance.GetComponent<CameraManager>();
        onRoomChange.Event += QueueEnemySpawn;
    }

    private void OnDisable()
    {
        onRoomChange.Event -= QueueEnemySpawn;
    }

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    #endregion

    private void QueueEnemySpawn(EventArgs argument)
    {
        if (argument is not RoomChangeEvent roomChangeEvent)
            return;
        if (roomChangeEvent.CurrentCamera != virtualCamera)
            return;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (;;)
        {
            if (enemyPoints <= 0)
                break;
            SpawnEnemy(out var head, out var body);
            InitializeEnemy(head, body);
        }
    }

    private void SpawnEnemy(out GameObject head, out GameObject body)
    {
        var enemyInfo = enemies[Random.Range(0, enemies.Length)];
        enemyPoints -= enemyInfo.PointValue;
        float spawnSizeX = spawnRange.size.x * spawnRange.transform.localScale.x / 2;
        float spawnSizeZ = spawnRange.size.z * spawnRange.transform.localScale.z / 2;
        float xOffset = Random.Range(-spawnSizeX, spawnSizeX);
        float zOffset = Random.Range(-spawnSizeZ, spawnSizeZ);
        var offset = Vector3.forward * zOffset + Vector3.right * xOffset;
        var spawnPosition = spawnRange.transform.position + offset;
        head = Instantiate(enemyInfo.Head, spawnPosition, quaternion.identity);
        body = Instantiate(enemyInfo.Body, spawnPosition, quaternion.identity);
        EnemyCount++;
    }

    private void InitializeEnemy(GameObject head, GameObject body)
    {
        head.GetComponent<Player>().PlayerID = Player.PlayerIdentifier.None;
        head.GetComponent<SkeletonController>().Recapitate(body);
        body.GetComponent<Health>().AddObserver(this);
    }

    public void OnNotify(Health.DamageTaken argument)
    {
        if (argument.Health > 0)
            return; 
        argument.Sender.GetComponent<Health>().RemoveObserver(this);
        EnemyCount--;
    }

    #region Sub-Classes
    [Serializable]
    private class EnemyDetails
    {
        [SerializeField, Min(1),] private int pointValue = 1;
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject head;
        
        public int PointValue => pointValue;
        public GameObject Body => body;
        public GameObject Head => head;
    }
    public class RoomChangeEvent : EventArgs
    {
        public RoomChangeEvent(CinemachineVirtualCamera currentCamera)
        {
            CurrentCamera = currentCamera;
        }

        public CinemachineVirtualCamera CurrentCamera { get; }
    }
    #endregion
}