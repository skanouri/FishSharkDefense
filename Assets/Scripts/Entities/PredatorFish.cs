using UnityEngine;

[RequireComponent(typeof(Fish))]
public class PredatorFish : MonoBehaviour
{
    [Header("Scan Settings")]
    public float scanInterval = 1.5f;   // 몇 초마다 먹잇감 탐색
    public float biteDistance = 0.3f;   // 이 거리 안에 오면 먹기
    public float chaseSpeedMultiplier = 1.5f;

    [Header("Level / Exp")]
    public int level = 1;
    public int maxLevel = 10;
    public int expPerEat = 1;
    public int expPerLevel = 3;

    private int _currentExp;
    private float _scanTimer;
    private Fish _selfFish;
    private Fish _currentTarget;

    private void Awake()
    {
        _selfFish = GetComponent<Fish>();
    }

    private void Start()
    {
        // 랜덤 수영 끄고 직접 이동 제어
        _selfFish.useExternalMovement = true;
    }

    private void Update()
    {
        if (FishManager.Instance == null || _selfFish.data == null)
            return;

        // 타겟이 없거나 유효하지 않으면 새로 탐색
        _scanTimer -= Time.deltaTime;
        if ((_currentTarget == null || !IsValidPrey(_currentTarget)) && _scanTimer <= 0f)
        {
            _scanTimer = scanInterval;
            _currentTarget = FindPrey();
        }

        // 타겟이 있으면 추격
        if (_currentTarget != null)
        {
            ChaseTarget();
        }
    }

    private void ChaseTarget()
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = _currentTarget.transform.position;

        float speed = _selfFish.data.moveSpeed * chaseSpeedMultiplier;
        transform.position = Vector2.MoveTowards(pos, targetPos, speed * Time.deltaTime);

        // 일정 거리 이내 → 먹기
        if (Vector2.Distance(pos, targetPos) <= biteDistance)
        {
            EatCurrentTarget();
        }
    }

    private bool IsValidPrey(Fish fish)
    {
        if (fish == null || fish.data == null) return false;
        if (fish == _selfFish) return false;

        // 먹잇감 타입: Normal / Spawner만
        if (fish.data.type != FishType.Normal && fish.data.type != FishType.Spawner)
            return false;

        // 티어 제한 (Predator의 사냥 가능한 최대 티어)
        int maxTier = _selfFish.data.predatorTargetMaxTier;
        if (maxTier > 0 && fish.data.tier > maxTier)
            return false;

        return true;
    }

    private Fish FindPrey()
    {
        Fish best = null;
        float bestTier = float.MaxValue;
        float bestDist = float.MaxValue;

        var list = FishManager.Instance.Fishes;
        foreach (var f in list)
        {
            if (!IsValidPrey(f)) continue;

            // 가장 낮은 티어 우선, 같으면 가까운 놈
            if (f.data.tier < bestTier)
            {
                bestTier = f.data.tier;
                best = f;
                bestDist = Vector2.Distance(transform.position, f.transform.position);
            }
            else if (Mathf.Approximately(f.data.tier, bestTier))
            {
                float d = Vector2.Distance(transform.position, f.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = f;
                }
            }
        }

        return best;
    }

    private void EatCurrentTarget()
    {
        if (_currentTarget == null) return;

        // 여기서 이펙트/애니메이션 추후 추가 가능
        Fish eaten = _currentTarget;
        _currentTarget = null;

        if (FishManager.Instance != null)
            FishManager.Instance.DestroyFish(eaten);
        else
            Destroy(eaten.gameObject);

        GainExp(expPerEat);
    }

    private void GainExp(int amount)
    {
        if (level >= maxLevel) return;

        _currentExp += amount;
        while (_currentExp >= expPerLevel && level < maxLevel)
        {
            _currentExp -= expPerLevel;
            level++;
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        // 일단은 디버그만, 나중에 공격력/코인 증가에 연결
        Debug.Log($"[Predator] {name} Level Up! -> Lv.{level}");
        // 예: _selfFish.data.coinPerSecond *= 1.1f; (SO 공유 문제 있으니 나중에 인스턴스 별 스탯으로 빼도 됨)
    }
}
