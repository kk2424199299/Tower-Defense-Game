using UnityEngine;

public class TurretUpgrade : MonoBehaviour
{
    [Header("升级属性")]
    public int Level = 1;
    public int UpgradeCost = 100;   // 升级一次要花多少钱
    public int SellValue = 50;      // 卖掉能回多少钱

    [Header("升级效果")]
    public float UpgradeDelayReduce = 0.2f; // 每次升级减少 0.2秒 的间隔

    private TurretProjectile _turretProjectile;

    private void Start()
    {
        _turretProjectile = GetComponent<TurretProjectile>();
    }

    public void Upgrade()
    {
        // 1. 数据变更
        Level++;
        UpgradeCost += 50; // 下一次升级更贵
        SellValue += 30;   // 卖价更高
        
        // 2. 增强能力：射速变快
        if (_turretProjectile != null)
        {
            _turretProjectile.delayBtwAttacks -= UpgradeDelayReduce;
            
            // 限制一下，别减成负数了（最快 0.1秒一发）
            if (_turretProjectile.delayBtwAttacks < 0.1f)
            {
                _turretProjectile.delayBtwAttacks = 0.1f;
            }
        }

        Debug.Log($"塔升级到了 Level {Level}！射击间隔变成了: {_turretProjectile.delayBtwAttacks}");
    }

    public void Sell()
    {
        // 加钱
        LevelManager.Instance.AddCurrency(SellValue);
        
        // 告诉 PlaceTurret 把这个坑位腾出来
        PlaceTurret placeTurret = FindObjectOfType<PlaceTurret>();
        if (placeTurret != null)
        {
            placeTurret.RemoveTurret(this);
        }

        // 销毁自己
        Destroy(gameObject);
        
        // 关闭面板
        if (NodeUI.Instance != null) NodeUI.Instance.Hide();
    }
}