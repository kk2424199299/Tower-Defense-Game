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

    [Header("Music")]
    // 只有 Boss 那一波需要填这个，普通波次留空即可！
    public AudioClip waveBGM; 
}

public class Spawner : MonoBehaviour
{
    [Header("Level Music")]
    // 【新增】这一关默认的背景音乐（比如轻松的战斗音乐）
    // 游戏一开始就会播放这个，替换掉主菜单的音乐
    public AudioClip levelBGM;

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
    private bool _allWavesSpawned = false; 
    private ObjectPooler _pooler;

    private void Start()
    {
        // 1. 初始化数据
        Enemy.TotalEnemiesAlive = 0;
        _pooler = GetComponent<ObjectPooler>();
        _currentWaveIndex = 0;
        _isSpawning = true; 
        _allWavesSpawned = false;
        
        UpdateWaveUI();

        // 2. 【关键】进入关卡，先播放“关卡默认BGM”
        if (levelBGM != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(levelBGM);
        }

        // 3. 检查第一波有没有特殊音乐（极少情况第一波就是Boss）
        CheckWaveMusic();
    }

    private void Update()
    {
        if (_allWavesSpawned)
        {
            if (Enemy.TotalEnemiesAlive <= 0)
            {
                Victory();
            }
            return; 
        }

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
                
                // 开始新的一波
                CheckWaveMusic();
            }
        }
    }

    // 【修改】检查是否需要换歌
    private void CheckWaveMusic()
    {
        if (_currentWaveIndex < waves.Count)
        {
            AudioClip specificMusic = waves[_currentWaveIndex].waveBGM;

            // 逻辑：只有当这一波“明确配置了音乐”时，才切换
            // 如果是空的（普通波次），就继续放之前的 Level BGM，不要打断
            if (specificMusic != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic(specificMusic);
            }
        }
    }

    private void SpawnCurrentWave()
    {
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

        if (_currentWaveIndex >= waves.Count)
        {
            _allWavesSpawned = true;
            Debug.Log("所有怪物已派出，等待玩家清除...");
        }
    }

    private void Victory()
    {
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
            int displayIndex = Mathf.Min(_currentWaveIndex + 1, waves.Count);
            waveText.text = $"WAVE: {displayIndex} / {waves.Count}";
        }
    }
}