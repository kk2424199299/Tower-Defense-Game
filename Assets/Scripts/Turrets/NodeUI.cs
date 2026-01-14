using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeUI : MonoBehaviour
{
    public static NodeUI Instance;

    [Header("UI 组件")]
    public GameObject uiPanel; // 面板本身
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellValueText;
    public Button upgradeButton;

    private TurretUpgrade _target; // 当前选中的塔

    private void Awake()
    {
        Instance = this;
        uiPanel.SetActive(false); // 游戏开始先隐藏
    }

    // 当你点击塔的时候，会调用这个方法
    public void SetTarget(TurretUpgrade target)
    {
        _target = target;

        // 设置显示位置 (可选：让面板飞到塔的位置，或者保持在固定位置)
        // transform.position = target.transform.position; 

        // 更新文字
        upgradeCostText.text = "$" + target.UpgradeCost;
        sellValueText.text = "$" + target.SellValue;

        // 检查钱够不够，不够就禁用按钮
        if (LevelManager.Instance.TotalCurrency < target.UpgradeCost)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }

        uiPanel.SetActive(true); // 显示面板
    }

    public void Hide()
    {
        uiPanel.SetActive(false);
    }

    // 绑定到 Upgrade 按钮
    public void Upgrade()
    {
        if (LevelManager.Instance.TotalCurrency >= _target.UpgradeCost)
        {
            LevelManager.Instance.RemoveCurrency(_target.UpgradeCost);
            _target.Upgrade();
            Hide(); // 升级完关闭面板
        }
    }

    // 绑定到 Sell 按钮
    public void Sell()
    {
        _target.Sell();
        Hide();
    }
}