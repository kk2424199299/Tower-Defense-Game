using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Turret Prefabs")]
    public GameObject standardTurretPrefab;
    public GameObject machineGunTurretPrefab;
    public GameObject tankTurretPrefab;

    [Header("Costs")]
    public int standardTurretCost = 100;
    public int machineGunTurretCost = 250;
    public int tankTurretCost = 500;

    private BuildManager _buildManager;

    private void Start()
    {
        _buildManager = BuildManager.Instance;
    }

    // --- 下面这些方法要绑定到 UI 按钮上 ---

    public void SelectStandardTurret()
    {
        // 告诉 BuildManager：我选中了标准塔，价格是 100
        _buildManager.SelectTurretToBuild(standardTurretPrefab, standardTurretCost);
    }

    public void SelectMachineGunTurret()
    {
        _buildManager.SelectTurretToBuild(machineGunTurretPrefab, machineGunTurretCost);
    }

    public void SelectTankTurret()
    {
        _buildManager.SelectTurretToBuild(tankTurretPrefab, tankTurretCost);
    }
}