using UnityEngine;

[RequireComponent(typeof(Fish))]
public class SpawnerFish : MonoBehaviour
{
    [Header("Spawn Settings")]
    public FishData spawnTargetData;    // 생성할 물고기 종류 (ex: 꼬마금붕어)
    public float spawnInterval = 10f;   // 몇 초마다 한 마리씩
    public int maxSpawnCount = 0;       // 0이면 무제한

    [Header("Spawn Offset")]
    public float spawnOffsetRadius = 0.5f;

    private int _spawnedCount;
    private float _timer;

    private void Update()
    {
        if (spawnTargetData == null || FishManager.Instance == null)
            return;

        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer -= spawnInterval;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (maxSpawnCount > 0 && _spawnedCount >= maxSpawnCount)
            return;

        // 약간 주변에 생성
        Vector2 basePos = transform.position;
        Vector2 offset = Random.insideUnitCircle * spawnOffsetRadius;
        Vector2 spawnPos = basePos + offset;

        // FishManager를 통해 생성
        Fish newFish = FishManager.Instance.SpawnFishAtPosition(spawnTargetData, spawnPos);
        if (newFish != null)
        {
            _spawnedCount++;
        }
    }
}
