using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 现在代码会听从 Inspector 面板的设置了
    // 所以请确保面板里填的是 Speed=20, Distance=1.5
    [SerializeField] private float moveSpeed = 20f; 
    [SerializeField] private float damage = 2f;
    [SerializeField] private float minDistanceToDealDamage = 1.5f; 
    [SerializeField] private float maxLifeTime = 4.0f; 

    public static Action<Enemy, float> OnEnemyHit;
    private Enemy _enemyTarget;
    private float _lifeTimer;
    private bool _hasFired = false; 

    private void OnEnable()
    {
        _lifeTimer = maxLifeTime;
        _hasFired = false;
    }

    private void Update()
    {
        if (!_hasFired) return;

        if (_enemyTarget == null)
        {
            ObjectPooler.ReturnToPool(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position,
            _enemyTarget.transform.position, moveSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, _enemyTarget.transform.position);
        
        // 这里会使用你在面板里填的数值 (1.5)
        if (distance < minDistanceToDealDamage)
        {
            HitEnemy();
        }

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void HitEnemy()
    {
        if (_enemyTarget.EnemyHealth != null)
        {
            _enemyTarget.EnemyHealth.DealDamage(damage);
        }

        try {
            OnEnemyHit?.Invoke(_enemyTarget, damage);
        } catch { }

        ObjectPooler.ReturnToPool(gameObject);
    }

    public void SetEnemy(Enemy enemy)
    {
        _enemyTarget = enemy;
        _hasFired = true; 
    }

    public void ResetProjectile()
    {
        _enemyTarget = null;
        _hasFired = false; 
        transform.localRotation = Quaternion.identity;
    }
}