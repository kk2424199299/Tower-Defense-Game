using UnityEngine;

[System.Serializable] // 这一行非常重要！加上它，你才能在 Inspector 里看到折叠菜单
public class TurretBlueprint
{
    [Header("基础设置")]
    public GameObject prefab;  // 基础塔的预制体
    public int cost;           // 造价

    [Header("升级设置")]
    public GameObject upgradedPrefab; // 升级后的塔预制体
    public int upgradeCost;           // 升级费用

    // 计算卖出价格 (比如打5折)
    public int GetSellAmount()
    {
        return cost / 2;
    }
}