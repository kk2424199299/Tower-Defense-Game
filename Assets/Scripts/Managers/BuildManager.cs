using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("场景里有多个 BuildManager!");
            return;
        }
        Instance = this;
    }

    // 【修改】这里不再存 GameObject，而是存图纸
    private TurretBlueprint _turretToBuild;
    private Node _selectedNode;

    // 建造特效 (可选)
    public GameObject buildEffect;
    public GameObject sellEffect;

    // --- 属性 ---

    // 检查是否有权建造
    public bool CanBuild { get { return _turretToBuild != null; } }
    // 检查钱够不够
    public bool HasMoney { get { return LevelManager.Instance.TotalCurrency >= _turretToBuild.cost; } }

    // --- 方法 ---

    // 让外部 (Node) 获取当前要造什么
    public TurretBlueprint GetTurretToBuild()
    {
        return _turretToBuild;
    }

    // 让外部 (Shop) 选定要造什么
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        _turretToBuild = turret;
        _selectedNode = null; // 选了造塔，就取消选中底座
    }

    // 选中地图上的底座 (用于升级/出售)
    public void SelectNode(Node node)
    {
        if (_selectedNode == node)
        {
            DeselectNode();
            return;
        }

        _selectedNode = node;
        _turretToBuild = null; // 选了底座，就取消造塔模式

        // 呼叫 UI 显示
        NodeUI.Instance.SetTarget(node);
    }

    public void DeselectNode()
    {
        _selectedNode = null;
        NodeUI.Instance.Hide();
    }
    
    // 重置状态
    public void ResetBuild()
    {
        _selectedNode = null;
        _turretToBuild = null;
        NodeUI.Instance.Hide();
    }
}