using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISharkHp : MonoBehaviour
{
    public GameObject rootPanel;       // SharkHpPanel
    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    private void Start()
    {
        // 시작할 때는 무조건 숨김
        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    private void Update()
    {
        var gm = GameManager.Instance;
        var shark = gm != null ? gm.currentShark : null;

        if (shark == null)
        {
            if (rootPanel != null && rootPanel.activeSelf)
                rootPanel.SetActive(false);
            return;
        }

        if (rootPanel != null && !rootPanel.activeSelf)
            rootPanel.SetActive(true);

        float ratio = shark.GetHpRatio();

        if (hpSlider != null)
            hpSlider.value = ratio;

        if (hpText != null)
        {
            int percent = Mathf.CeilToInt(ratio * 100f);
            hpText.text = $"Shark HP: {percent}%";
        }
    }
}
