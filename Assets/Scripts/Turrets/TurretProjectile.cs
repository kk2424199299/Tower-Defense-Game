using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnPosition;
    
    // 【关键】必须是 public，这样升级脚本才能修改它
    public float delayBtwAttacks = 2f; 
    
    private float _nextAttackTime;
    private ObjectPooler _pooler;
    private Turret _turret;
    private Projectile _currentProjectileLoaded;

    private void Start()
    {
        _turret = GetComponent<Turret>();
        _pooler = GetComponent<ObjectPooler>();
        
        LoadProjectile();
    }

    private void Update()
    {
        if (_currentProjectileLoaded == null)
        {
            LoadProjectile();
        }

        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null &&
                _turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
            {
                _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);
                _currentProjectileLoaded = null; 
                _nextAttackTime = Time.time + delayBtwAttacks;
            }
        }
    }

    private void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        if (newInstance == null) return;

        newInstance.transform.position = projectileSpawnPosition.position;
        newInstance.transform.rotation = projectileSpawnPosition.rotation;
        
        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        
        // 【已修复】删除了报错的那一行 (TurretOwner)
        // _currentProjectileLoaded.TurretOwner = this; 

        newInstance.SetActive(true);
    }

    public void ResetTurretProjectile()
    {
        _currentProjectileLoaded = null;
    }
}