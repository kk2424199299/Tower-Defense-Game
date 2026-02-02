using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [Header("颜色设置")]
    public Color hoverColor;          // 鼠标悬停时的颜色 (建议设为浅灰色)
    public Color notEnoughMoneyColor; // 钱不够时的颜色 (建议设为红色)
    private Color startColor;
    private Renderer rend;

    [Header("状态")]
    public GameObject currentTurret;        // 当前身上的塔
    public TurretBlueprint turretBlueprint; // 记录当前塔的图纸
    public bool isUpgraded = false;         // 记录是否升级过

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position;
    }

    private void OnMouseEnter()
    {
        // 1. 如果点在 UI 上，或者是空的 BuildManager，就不变色
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (BuildManager.Instance.GetTurretToBuild() == null) return;

        // 2. 根据钱够不够，显示不同的颜色
        if (LevelManager.Instance.TotalCurrency >= BuildManager.Instance.GetTurretToBuild().cost)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    private void OnMouseDown()
    {
        // 防止点穿UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // A. 如果这里已经有塔了 -> 呼叫 NodeUI 面板！
        if (currentTurret != null)
        {
            if (NodeUI.Instance != null)
            {
                NodeUI.Instance.SetTarget(this);
            }
            return;
        }

        // B. 如果没塔，且手里选了图纸 -> 建造
        if (BuildManager.Instance.GetTurretToBuild() == null) return;

        BuildTurret(BuildManager.Instance.GetTurretToBuild());
    }

    // --- 建造逻辑 ---
    public void BuildTurret(TurretBlueprint blueprint)
    {
        // 检查钱够不够 (使用 LevelManager)
        if (LevelManager.Instance.TotalCurrency < blueprint.cost)
        {
            Debug.Log("钱不够！无法建造");
            return;
        }

        // 扣钱
        LevelManager.Instance.RemoveCurrency(blueprint.cost);

        // 生成塔
        GameObject _turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        currentTurret = _turret;
        turretBlueprint = blueprint;

        Debug.Log("建造成功！");

        // 【关键修复】造完塔后，放下手里的图纸，防止连续建造
        BuildManager.Instance.ResetBuild();
    }

    // --- 升级逻辑 ---
    public void UpgradeTurret()
    {
        if (LevelManager.Instance.TotalCurrency < turretBlueprint.upgradeCost)
        {
            Debug.Log("钱不够！无法升级");
            return;
        }

        LevelManager.Instance.RemoveCurrency(turretBlueprint.upgradeCost);

        // 销毁旧塔，生成新塔
        Destroy(currentTurret);
        GameObject _turret = Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        currentTurret = _turret;

        isUpgraded = true;
        Debug.Log("升级成功！");
    }

    // --- 出售逻辑 ---
    public void SellTurret()
    {
        LevelManager.Instance.AddCurrency(turretBlueprint.GetSellAmount());

        Destroy(currentTurret);
        turretBlueprint = null;
        isUpgraded = false;
    }
}