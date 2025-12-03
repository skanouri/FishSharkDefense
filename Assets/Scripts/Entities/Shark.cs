using UnityEngine;
using UnityEngine.EventSystems;


public class Shark : MonoBehaviour, IPointerDownHandler
{
    [Header("Stats")]
    public float maxHp = 100f;
    public float moveSpeed = 1.5f;
    public float dpsMultiplier = 1f;   // 물고기 공격력 전체에 곱해줄 계수

    [Header("Rewards")]
    public long coinReward = 100;
    public long pearlReward = 1;

    [Header("Click Damage")]
    public float clickDamage = 5f;     // 유저가 클릭할 때마다 들어가는 대미지

    private float _currentHp;
    private Vector3 _startPos;
    private float _swimTimer;

    private void OnEnable()
    {
        _currentHp = maxHp;
        _startPos = transform.position;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentShark(this);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentShark == this)
        {
            GameManager.Instance.SetCurrentShark(null);
        }
    }

    private void Update()
    {
        ApplyFishDamage();
        SimpleSwim();
    }

    private void ApplyFishDamage()
    {
        if (FishManager.Instance == null) return;

        float totalDps = 0f;
        var fishes = FishManager.Instance.Fishes;
        for (int i = 0; i < fishes.Count; i++)
        {
            var f = fishes[i];
            if (f == null || f.data == null) continue;

            // 간단 MVP: 모든 물고기 공격력 합산
            totalDps += f.data.baseAttack;
        }

        if (totalDps <= 0f) return;

        float damage = totalDps * dpsMultiplier * Time.deltaTime;
        TakeDamage(damage);
    }

    private void SimpleSwim()
    {
        // 그냥 좌우로 천천히 왔다 갔다 하는 정도의 MVP용 이동
        _swimTimer += Time.deltaTime;
        float offsetX = Mathf.Sin(_swimTimer * 0.5f) * 1.5f;
        float offsetY = Mathf.Sin(_swimTimer * 0.8f) * 0.3f;

        transform.position = _startPos + new Vector3(offsetX, offsetY, 0f);
    }

    public void TakeDamage(float amount)
    {
        if (_currentHp <= 0f) return;

        _currentHp -= amount;
        if (_currentHp <= 0f)
        {
            _currentHp = 0f;
            Die();
        }
    }

    private void Die()
    {
        // 보상 지급
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin(coinReward);
            GameManager.Instance.AddPearl(pearlReward);
        }

        // 나중에 폭발 이펙트, 애니메이션 등
        Destroy(gameObject);
    }

    public float GetHpRatio()
    {
        if (maxHp <= 0f) return 0f;
        return Mathf.Clamp01(_currentHp / maxHp);
    }

    // 🔫 유저 클릭 → 대미지 (PC + 모바일 모두 어느 정도 동작)
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("take damage");
        // 클릭 시 대미지
        TakeDamage(clickDamage);
    }
}
