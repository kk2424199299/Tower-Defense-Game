using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float attackRange = 4f; // 攻击范围
    [SerializeField] private float rotationSpeed = 200f; // 旋转速度

    public Enemy CurrentEnemyTarget { get; set; }

    private bool _gameStarted;

    private void Start()
    {
        _gameStarted = true;
    }

    private void Update()
    {
        if (!_gameStarted) return;

        // 1. 如果当前有目标，检查它是否还合法
        if (CurrentEnemyTarget != null)
        {
            // 如果目标死了，或者超出攻击范围，就放弃它
            if (CurrentEnemyTarget.EnemyHealth.CurrentHealth <= 0 || 
                Vector2.Distance(transform.position, CurrentEnemyTarget.transform.position) > attackRange)
            {
                CurrentEnemyTarget = null;
            }
        }

        // 2. 如果当前没目标（或者刚才放弃了），才去寻找新目标
        if (CurrentEnemyTarget == null)
        {
            FindClosestEnemy();
        }

        // 3. 瞄准目标
        if (CurrentEnemyTarget != null)
        {
            RotateTowardsTarget();
        }
    }

    private void FindClosestEnemy()
    {
        // 获取范围内所有敌人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Collider2D collider in enemies)
        {
            // 确保找到的是敌人
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemyScript = collider.GetComponent<Enemy>();
                // 确保敌人是活着的
                if (enemyScript.EnemyHealth.CurrentHealth > 0)
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemyScript;
                    }
                }
            }
        }

        // 锁定这个最近的敌人
        if (nearestEnemy != null)
        {
            CurrentEnemyTarget = nearestEnemy;
        }
    }

    private void RotateTowardsTarget()
    {
        // 计算角度
        Vector3 direction = CurrentEnemyTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        
        // 平滑旋转
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    // 在编辑器里画出攻击范围圆圈，方便调试
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}