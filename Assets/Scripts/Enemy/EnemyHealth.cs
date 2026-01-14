using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private int currencyWorth = 10; 

    [Header("UI References")]
    [SerializeField] private Image healthBarFill; 

    public float CurrentHealth { get; set; }

    private Animator _anim;
    private bool _isDead = false;

    private void OnEnable()
    {
        CurrentHealth = initialHealth;
        _isDead = false;
        
        if (healthBarFill != null) healthBarFill.fillAmount = 1f;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        Enemy enemyMovement = GetComponent<Enemy>();
        if (enemyMovement != null) enemyMovement.enabled = true;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void DealDamage(float damageAmount)
    {
        if (_isDead) return;

        CurrentHealth -= damageAmount;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = CurrentHealth / initialHealth;
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        CurrentHealth = 0;
        
        if (healthBarFill != null) healthBarFill.fillAmount = 0f;

        // 1. 加钱
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.AddCurrency(currencyWorth);
        }

        // 2. 【关键】通知成就系统！
        Enemy.TriggerDeathEvent();

        // 3. 播放死亡动画
        if (_anim != null)
        {
            _anim.SetTrigger("Die");
        }

        // 4. 禁用移动和碰撞
        Enemy enemyMovement = GetComponent<Enemy>();
        if (enemyMovement != null) enemyMovement.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 5. 延迟回收
        StartCoroutine(ReturnToPoolAfterDelay(0.5f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void ResetHealth()
    {
        CurrentHealth = initialHealth;
        _isDead = false;
        if (healthBarFill != null) healthBarFill.fillAmount = 1f;
        
        // 重置动画状态，防止复活时还是倒地状态
        if (_anim != null)
        {
            _anim.Play("Move"); // 确保有一段叫 "Move" 或 "Run" 的走路动画
        }
    }
}