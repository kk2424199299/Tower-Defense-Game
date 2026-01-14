using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; 

[System.Serializable]
public class Wave
{
    public string enemyTag; 
    public int count;       
    public float spawnInterval; 
}

public class Spawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Wave> waves; 
    public float timeBetweenWaves = 5f; 

    [Header("UI References")]
    public TextMeshProUGUI waveText; 

    [Header("References")]
    public Transform spawnPoint; 

    private float _spawnTimer;
    private int _currentWaveIndex = 0;
    private int _enemiesSpawnedInCurrentWave = 0;
    private bool _isSpawning = false;
    private bool _allWavesSpawned = false; // 【新增】标记：是否所有怪都出笼了？
    private ObjectPooler _pooler;

    private void Start()
    {
        // 安全起见，游戏开始时重置计数器 (防止换场景时残留)
        Enemy.TotalEnemiesAlive = 0;

        _pooler = GetComponent<ObjectPooler>();
        _currentWaveIndex = 0;
        _isSpawning = true; 
        _allWavesSpawned = false;
        
        UpdateWaveUI();
    }

    private void Update()
    {
        // 1. 如果所有波次都刷完了，我们就进入“等待清场”模式
        if (_allWavesSpawned)
        {
            // 检查场上还有没有活人
            if (Enemy.TotalEnemiesAlive <= 0)
            {
                Victory();
            }
            return; // 不再执行下面的刷怪逻辑
        }

        // 2. 正常的刷怪循环
        if (_isSpawning)
        {
            SpawnCurrentWave();
        }
        else
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                _isSpawning = true;
                _enemiesSpawnedInCurrentWave = 0;
                UpdateWaveUI();
            }
        }
    }

    private void SpawnCurrentWave()
    {
        // 防止索引越界
        if (_currentWaveIndex >= waves.Count) return;

        Wave currentWave = waves[_currentWaveIndex];

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= currentWave.spawnInterval)
        {
            _spawnTimer = 0;
            SpawnEnemy(currentWave.enemyTag);
            _enemiesSpawnedInCurrentWave++;

            if (_enemiesSpawnedInCurrentWave >= currentWave.count)
            {
                FinishWave();
            }
        }
    }

    private void SpawnEnemy(string enemyTag)
    {
        GameObject newEnemy = _pooler.GetInstanceFromPool(enemyTag);
        
        if (newEnemy != null)
        {
            if (spawnPoint != null) newEnemy.transform.position = spawnPoint.position;
            
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            if (enemyScript != null) enemyScript.ResetEnemy(); 
            
            newEnemy.SetActive(true);
        }
    }

    private void FinishWave()
    {
        _isSpawning = false; 
        _spawnTimer = timeBetweenWaves; 
        _currentWaveIndex++; 

        // 检查是否全部波次都打完了
        if (_currentWaveIndex >= waves.Count)
        {
            // 【修改】这里不再直接胜利，而是只标记“怪刷完了”
            _allWavesSpawned = true;
            Debug.Log("所有怪物已派出，等待玩家清除...");
        }
    }

    private void Victory()
    {
        // 防止重复调用
        this.enabled = false; 

        Debug.Log("场上怪物已清空，胜利！");
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LevelCompleted();
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            // 显示时防止索引溢出
            int displayIndex = Mathf.Min(_currentWaveIndex + 1, waves.Count);
            waveText.text = $"WAVE: {displayIndex} / {waves.Count}";
        }
    }
}