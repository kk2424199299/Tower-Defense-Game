using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("塔的图纸配置")]
    // 1. 标准塔
    public TurretBlueprint standardTurret;
    
    // 2. 坦克塔 (Tank)
    public TurretBlueprint tankTurret;
    
    // 3. 机枪塔 (Machine Gun)
    public TurretBlueprint machineGunTurret;

    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.Instance;
    }

    // --- 按钮点击事件 (记得去Unity里重新绑定!) ---

    // 按钮 1：选中标准塔
    public void SelectStandardTurret()
    {
        Debug.Log("选中了标准塔 (Standard)");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    // 按钮 2：选中坦克塔
    public void SelectTankTurret()
    {
        Debug.Log("选中了坦克塔 (Tank)");
        buildManager.SelectTurretToBuild(tankTurret);
    }

    // 按钮 3：选中机枪塔
    public void SelectMachineGunTurret()
    {
        Debug.Log("选中了机枪塔 (Machine Gun)");
        buildManager.SelectTurretToBuild(machineGunTurret);
    }
}