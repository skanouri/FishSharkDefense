using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Fish : MonoBehaviour
{
    [Header("Data")]
    public FishData data;

    [Header("Movement")]
    public float moveRangeX = 7f;
    public float moveRangeY = 3f;
    public float changeDirInterval = 2f;

    // === 추가: 외부에서 이동 제어할지 여부(포식자용) ===
    [HideInInspector] public bool useExternalMovement = false;

    private SpriteRenderer _sr;
    private Vector2 _targetPos;
    private float _dirTimer;
    private float _coinTimer;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (FishManager.Instance != null)
            FishManager.Instance.RegisterFish(this);
    }

    private void OnDisable()
    {
        if (FishManager.Instance != null)
            FishManager.Instance.UnregisterFish(this);
    }

    private void Start()
    {
        if (data != null && data.sprite != null)
        {
            _sr.sprite = data.sprite;
        }

        SetNewTargetPosition();
    }

    private void Update()
    {
        // 🔥 포식자는 외부 이동 사용 → 여기서 랜덤수영 비활성화
        if (!useExternalMovement)
        {
            MoveUpdate();
        }

        CoinUpdate();
    }

    private void MoveUpdate()
    {
        _dirTimer -= Time.deltaTime;

        transform.position = Vector2.MoveTowards(
            transform.position,
            _targetPos,
            data.moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, _targetPos) < 0.1f || _dirTimer <= 0f)
        {
            SetNewTargetPosition();
        }

        if (_targetPos.x < transform.position.x) _sr.flipX = true;
        else _sr.flipX = false;
    }

    private void SetNewTargetPosition()
    {
        _dirTimer = changeDirInterval + Random.Range(-0.5f, 0.5f);

        var origin = Vector2.zero;
        float tx = origin.x + Random.Range(-moveRangeX, moveRangeX);
        float ty = origin.y + Random.Range(-moveRangeY, moveRangeY);
        _targetPos = new Vector2(tx, ty);
    }

    private void CoinUpdate()
    {
        if (data == null || data.coinPerSecond <= 0f) return;

        _coinTimer += Time.deltaTime;
        if (_coinTimer >= 1f)
        {
            _coinTimer -= 1f;
            long amount = (long)Mathf.RoundToInt(data.coinPerSecond);
            GameManager.Instance?.AddCoin(amount);
        }
    }
}
