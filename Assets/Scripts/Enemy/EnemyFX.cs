using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    [SerializeField] private Transform textDamageSpawnPosition;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // 当收到“有敌人被打了”的通知时
    private void EnemyHit(Enemy enemy, float damage)
    {
        // 检查一下：被打的那个敌人是不是我自己？
        if (_enemy == enemy)
        {
            // 1. 从管理器那里拿一个飘字
            GameObject newInstance = DamageTextManager.Instance.Pooler.GetInstanceFromPool();
            
            // 2. 设置文字内容（显示伤害数值）
            TextMeshProUGUI damageText = newInstance.GetComponent<DamageText>().DmgText;
            damageText.text = damage.ToString();
            
            // 3. 设置位置（放到头顶那个点）
            newInstance.transform.SetParent(textDamageSpawnPosition);
            newInstance.transform.position = textDamageSpawnPosition.position;
            newInstance.SetActive(true);
        }
    }
    
    // 注册和注销事件监听（必须写！）
    private void OnEnable()
    {
        Projectile.OnEnemyHit += EnemyHit;
    }
    private void OnDisable()
    {
        Projectile.OnEnemyHit -= EnemyHit;
    }
}