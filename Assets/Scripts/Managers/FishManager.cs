using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager Instance { get; private set; }

    [Header("Prefabs")]
    public Fish fishPrefab;

    [Header("Spawn Area")]
    public Transform spawnCenter;   // 탱크 중앙 기준
    public float spawnRangeX = 7f;
    public float spawnRangeY = 3f;

    [Header("Limit")]
    public int maxFishCount = 100;

    // === 추가: 현재 물고기 목록 관리 ===
    private readonly List<Fish> _fishes = new List<Fish>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 외부에서 읽기용
    public IReadOnlyList<Fish> Fishes => _fishes;

    public void RegisterFish(Fish fish)
    {
        if (fish == null) return;
        if (!_fishes.Contains(fish))
            _fishes.Add(fish);
    }

    public void UnregisterFish(Fish fish)
    {
        if (fish == null) return;
        _fishes.Remove(fish);
    }

    public int CountCurrentFish()
    {
        return _fishes.Count;
    }

    public Fish SpawnFish(FishData data)
    {
        if (CountCurrentFish() >= maxFishCount)
        {
            Debug.Log("[FishManager] Max fish count reached.");
            return null;
        }

        if (fishPrefab == null || data == null)
        {
            Debug.LogWarning("[FishManager] Missing prefab or data.");
            return null;
        }

        Vector2 center = spawnCenter != null ? (Vector2)spawnCenter.position : Vector2.zero;
        Vector2 pos = center + new Vector2(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(-spawnRangeY, spawnRangeY)
        );

        Fish f = Instantiate(fishPrefab, pos, Quaternion.identity, transform);
        f.data = data;
        return f;
    }

    public Fish SpawnFishAtPosition(FishData data, Vector2 position)
    {
        if (CountCurrentFish() >= maxFishCount)
        {
            Debug.Log("[FishManager] Max fish count reached.");
            return null;
        }

        if (fishPrefab == null || data == null)
        {
        Debug.LogWarning("[FishManager] Missing prefab or data.");
            return null;
        }

        Fish f = Instantiate(fishPrefab, position, Quaternion.identity, transform);
        f.data = data;
        return f;
    }

    public void DestroyFish(Fish fish)
    {
        if (fish == null) return;
        UnregisterFish(fish);
        Destroy(fish.gameObject);
    }
}
