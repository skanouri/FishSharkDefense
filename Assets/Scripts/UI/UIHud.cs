using TMPro;
using UnityEngine;

public class UIHud : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI pearlText;

    private void Start()
    {
        // 처음 한 번 갱신
        RefreshAll();
    }

    private void Update()
    {
        // MVP 단계에선 그냥 매 프레임 동기화
        // 나중에 최적화할 땐 이벤트 방식으로 바꿔도 됨
        if (GameManager.Instance != null)
        {
            coinText.text = GameManager.Instance.coin.ToString("N0");
            pearlText.text = GameManager.Instance.pearl.ToString("N0");
        }
    }

    public void RefreshAll()
    {
        if (GameManager.Instance == null) return;

        coinText.text = GameManager.Instance.coin.ToString("N0");
        pearlText.text = GameManager.Instance.pearl.ToString("N0");
    }
}
