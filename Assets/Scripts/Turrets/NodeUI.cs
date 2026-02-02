using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeUI : MonoBehaviour
{
    public static NodeUI Instance;

    [Header("UI 组件")]
    public GameObject uiPanel;        // 整个面板的父物体
    public TextMeshProUGUI upgradeCostText; // 显示升级价格的文本
    public TextMeshProUGUI sellValueText;   // 显示卖出价格的文本
    public Button upgradeButton;      // 升级按钮
    public Button sellButton;         // 卖出按钮

    private Node _target;             // 当前选中的底座

    private void Awake()
    {
        Instance = this;
        uiPanel.SetActive(false); // 游戏一开始先隐藏面板
    }

    // 当点击地图上的塔时，Node.cs 会调用这个方法
    public void SetTarget(Node target)
    {
        _target = target;

        // 注意：这里没有 transform.position = ... 的代码
        // 面板会保持在你 Inspector 里设置好的位置（比如右下角）

        // --- 获取数据并更新 UI ---
        
        // 如果底座上没有塔的数据，就隐藏
        if (_target.turretBlueprint == null)
        {
             Hide(); 
             return; 
        }

        // 1. 设置升级按钮状态
        if (!_target.isUpgraded)
        {
            upgradeCostText.text = "$" + _target.turretBlueprint.upgradeCost;
            
            // 检查钱够不够，不够就把按钮变灰
            if (LevelManager.Instance != null)
            {
                upgradeButton.interactable = LevelManager.Instance.TotalCurrency >= _target.turretBlueprint.upgradeCost;
            }
        }
        else
        {
            // 已经升级过了
            upgradeCostText.text = "MAX";
            upgradeButton.interactable = false;
        }

        // 2. 设置卖出价格
        sellValueText.text = "$" + _target.turretBlueprint.GetSellAmount();

        // 显示面板
        uiPanel.SetActive(true);
    }

    // 隐藏面板
    public void Hide()
    {
        uiPanel.SetActive(false);
    }

    // --- 按钮事件绑定 ---

    // 升级按钮点击时调用
    public void Upgrade()
    {
        if (_target != null)
        {
            _target.UpgradeTurret();
            BuildManager.Instance.ResetBuild(); // 升级完取消选中
            Hide();
        }
    }

    // 卖出按钮点击时调用
    public void Sell()
    {
        if (_target != null)
        {
            _target.SellTurret();
            BuildManager.Instance.ResetBuild(); // 卖完取消选中
            Hide();
        }
    }
}