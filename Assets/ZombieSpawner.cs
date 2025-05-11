using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;      // Assign in Inspector
    public Transform player;             // Assign in Inspector
    public int maxZombies = 20;
    public float spawnInterval = 60f;    // 1 minute
    public float minSpawnRadius = 50f;
    public float maxSpawnRadius = 70f;

    private int zombieCount = 0;
    private float spawnTimer = 0f;

    void Start()
    {
        SpawnZombieNearPlayer(); // 🔥 Spawn one at the start
        spawnTimer = 0f;
    }

    void Update()
    {
        if (zombieCount >= maxZombies) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnZombieNearPlayer();
            spawnTimer = 0f;
        }
    }

    void SpawnZombieNearPlayer()
    {
        if (zombieCount >= maxZombies) return;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        zombieCount++;
    }
}
