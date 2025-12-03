using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    public Shark sharkPrefab;
    public Transform spawnPoint;

    [Header("Timing")]
    public float firstSpawnDelay = 10f;
    public float spawnInterval = 30f;

    private float _timer;

    private void Start()
    {
        _timer = firstSpawnDelay;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        // 이미 상어가 있으면 타이머 멈춤
        if (GameManager.Instance.currentShark != null)
            return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnShark();
            _timer = spawnInterval;
        }
    }

    private void SpawnShark()
    {
        if (sharkPrefab == null)
        {
            Debug.LogWarning("[SharkSpawner] Shark prefab is null.");
            return;
        }

        Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Instantiate(sharkPrefab, pos, Quaternion.identity);
    }
}
