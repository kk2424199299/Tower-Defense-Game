using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 这些参数会覆盖 Inspector 里的设置，确保它们是你想要的数值
    [SerializeField] private float moveSpeed = 20f; 
    [SerializeField] private float damage = 1.5f;
    [SerializeField] private float minDistanceToDealDamage = 1.5f; 
    [SerializeField] private float maxLifeTime = 4.0f; 

    [Header("Audio")]
    // 【关键】这里用来放击中音效
    [SerializeField] private AudioClip hitSound; 

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

        // 飞向敌人
        transform.position = Vector2.MoveTowards(transform.position,
            _enemyTarget.transform.position, moveSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, _enemyTarget.transform.position);
        
        // 距离足够近，判定为击中
        if (distance < minDistanceToDealDamage)
        {
            HitEnemy();
        }

        // 超时回收
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void HitEnemy()
    {
        // 1. 扣血
        if (_enemyTarget.EnemyHealth != null)
        {
            _enemyTarget.EnemyHealth.DealDamage(damage);
        }

        // 2. 【核心逻辑】播放击中音效
        // PlaySFX 允许声音重叠，所以多个子弹同时打中也没问题
        if (AudioManager.Instance != null && hitSound != null)
        {
            AudioManager.Instance.PlaySFX(hitSound);
        }

        // 3. 触发飘字等特效
        try {
            OnEnemyHit?.Invoke(_enemyTarget, damage);
        } catch { }

        // 4. 子弹消失（回池）
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void SetEnemy(Enemy enemy)
    {
        _enemyTarget = enemy;
        _hasFired = true;
    }
}