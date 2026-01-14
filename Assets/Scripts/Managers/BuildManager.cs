using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    // 当前准备建造的塔（预制体）
    private GameObject _turretToBuild;
    
    // 当前塔的价格
    private int _turretCost;

    // 获取当前要造的塔
    public GameObject GetTurretToBuild()
    {
        return _turretToBuild;
    }
    
    // 获取当前塔的价格
    public int GetTurretCost()
    {
        return _turretCost;
    }

    // 商店选中某个塔时调用这个方法
    public void SelectTurretToBuild(GameObject turret, int cost)
    {
        _turretToBuild = turret;
        _turretCost = cost;
        Debug.Log("已选中: " + turret.name + " | 价格: " + cost);
    }
    
    // 检查是否处于“建造模式”（手里有没有塔）
    public bool CanBuild { get { return _turretToBuild != null; } }
    
    // 造完或者取消时，清空手里的塔
    public void ResetBuild()
    {
        _turretToBuild = null;
        _turretCost = 0;
    }
}