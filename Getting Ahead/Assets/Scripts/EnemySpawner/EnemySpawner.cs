using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class EnemySpawner : MonoBehaviour, IObserver<CinemachineVirtualCamera>
{
    [SerializeField] private EnemyDetails[] enemies;
    [SerializeField] private BoxCollider spawnRange;
    [SerializeField] private int enemyPoints;
    
    private CinemachineVirtualCamera virtualCamera;
    private CameraManager cameraManager;
    
    #region Unity Events
    private void OnEnable()
    {
        cameraManager = GameManager.instance.GetComponent<CameraManager>();
        cameraManager.AddObserver(this);
    }

    private void OnDisable()
    {
        cameraManager.RemoveObserver(this);
    }

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    #endregion

    public void OnNotify(CinemachineVirtualCamera activeCamera)
    {
        if (activeCamera != virtualCamera)
            return;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (;;)
        {
            if (enemyPoints <= 0)
                break;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyInfo = enemies[Random.Range(0, enemies.Length - 1)];
        enemyPoints -= enemyInfo.PointValue;
        float spawnSizeX = spawnRange.size.x * spawnRange.transform.localScale.x / 2;
        float spawnSizeZ = spawnRange.size.z * spawnRange.transform.localScale.z / 2;
        float xOffset = Random.Range(-spawnSizeX, spawnSizeX);
        float zOffset = Random.Range(-spawnSizeZ, spawnSizeZ);
        var offset = Vector3.forward * zOffset + Vector3.right * xOffset;
        var spawnPosition = spawnRange.transform.position + offset;
        var body = Instantiate(enemyInfo.Body, spawnPosition, quaternion.identity);
        var head = Instantiate(enemyInfo.Head, spawnPosition, quaternion.identity);
        head.GetComponent<SkeletonController>().Recapitate(body);
    }

    #region Sub-Classes
    [System.Serializable]
    private class EnemyDetails
    {
        [SerializeField, Min(1),] private int pointValue = 1;
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject head;
        
        public int PointValue => pointValue;
        public GameObject Body => body;
        public GameObject Head => head;
    }
    #endregion
}