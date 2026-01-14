using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class PlaceTurret : MonoBehaviour
{
    private Tilemap _tilemap;
    private Dictionary<Vector3Int, TurretUpgrade> _turrets = new Dictionary<Vector3Int, TurretUpgrade>();

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        // 右键取消
        if (Input.GetMouseButtonDown(1))
        {
            if (BuildManager.Instance.CanBuild) BuildManager.Instance.ResetBuild();
            if (NodeUI.Instance != null) NodeUI.Instance.Hide();
            return;
        }

        // 左键点击 (必须加 IsPointerOverGameObject 判断防止点穿 UI)
        if (Input.GetMouseButtonDown(0))
        {
            // 如果鼠标点在按钮上，直接不处理地图逻辑
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Debug.Log("点到了UI，忽略地图点击");
                return;
            }

            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int cellPos = _tilemap.WorldToCell(mouseWorldPos);

        // Debug.Log("鼠标点击格子: " + cellPos);

        // --- 【智能修正部分】 ---
        
        // 1. 先检查当前点击的格子有没有塔
        if (_turrets.ContainsKey(cellPos))
        {
            SelectTurret(cellPos);
            return;
        }
        
        // 2. 如果当前格子没塔，试试检查“脚下”那个格子 (Y - 1)
        // 因为塔的图片通常很高，玩家很容易点到塔身，实际上塔在下面
        Vector3Int cellBelow = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);
        if (_turrets.ContainsKey(cellBelow))
        {
            SelectTurret(cellBelow);
            return;
        }

        // --- 如果上面都找不到，说明点的是空地 ---

        if (BuildManager.Instance.CanBuild)
        {
            // 检查这里是否已经有塔（防止重叠建造）
            // 注意：这里我们不允许重叠，即使是在头顶位置
            if (!_turrets.ContainsKey(cellPos)) 
            {
                BuildTurret(cellPos);
            }
        }
        else
        {
            if (NodeUI.Instance != null) NodeUI.Instance.Hide();
        }
    }

    // 抽离出来的选中逻辑
    private void SelectTurret(Vector3Int pos)
    {
        TurretUpgrade selectedTurret = _turrets[pos];
        if (NodeUI.Instance != null)
        {
            NodeUI.Instance.SetTarget(selectedTurret);
        }
        BuildManager.Instance.ResetBuild();
    }

    private void BuildTurret(Vector3Int cellPos)
    {
        GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
        int turretCost = BuildManager.Instance.GetTurretCost();

        if (LevelManager.Instance.TotalCurrency < turretCost)
        {
            Debug.Log("金币不足！");
            return;
        }

        LevelManager.Instance.RemoveCurrency(turretCost);

        // 矫正Z轴，防止塔被地图遮挡
        Vector3 buildPos = _tilemap.GetCellCenterWorld(cellPos);
        buildPos.z = 0; 

        GameObject newTurret = Instantiate(turretToBuild, buildPos, Quaternion.identity);

        TurretUpgrade upgradeComponent = newTurret.GetComponent<TurretUpgrade>();
        if (upgradeComponent == null) upgradeComponent = newTurret.AddComponent<TurretUpgrade>();

        _turrets.Add(cellPos, upgradeComponent);
        BuildManager.Instance.ResetBuild();
    }

    public void RemoveTurret(TurretUpgrade turret)
    {
        Vector3Int keyToRemove = Vector3Int.zero;
        bool found = false;

        foreach (var pair in _turrets)
        {
            if (pair.Value == turret)
            {
                keyToRemove = pair.Key;
                found = true;
                break;
            }
        }

        if (found) _turrets.Remove(keyToRemove);
    }
}