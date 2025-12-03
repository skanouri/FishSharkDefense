using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Currency")]
    public long coin;
    public long pearl;

    [Header("Battle")]
    public Shark currentShark;   // 🔥 현재 전투 중인 상어

    private void Awake()
    {
        // 싱글톤 세팅
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        AddCoin(0);
        AddPearl(0);
    }

    public void SetCurrentShark(Shark shark)
    {
        currentShark = shark;
    }

    public void AddCoin(long amount)
    {
        coin += amount;
        // TODO: UI 업데이트 호출 (나중에)
    }

    public void AddPearl(long amount)
    {
        pearl += amount;
        // TODO: UI 업데이트 호출 (나중에)
    }
}
