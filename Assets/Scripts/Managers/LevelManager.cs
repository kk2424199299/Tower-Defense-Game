using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 必须引用

public class LevelManager : Singleton<LevelManager>
{
    [Header("Currency")]
    [SerializeField] private int startingCurrency = 100;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Lives")]
    [SerializeField] private int startingLives = 10;
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    [Header("Audio Clips")]
    // 【新增】金币音效 (记得把下载的音频拖进来)
    [SerializeField] private AudioClip coinSound; 

    public int TotalCurrency { get; set; }
    public int TotalLives { get; set; }

    private void Start()
    {
        // 强制恢复游戏时间
        Time.timeScale = 1f;

        TotalCurrency = startingCurrency;
        TotalLives = startingLives;
        
        UpdateCurrencyUI();
        UpdateLivesUI();
    }

    public void AddCurrency(int amount)
    {
        TotalCurrency += amount;
        UpdateCurrencyUI();

        // 【新增】播放金币音效
        if (AudioManager.Instance != null && coinSound != null)
        {
            AudioManager.Instance.PlaySFX(coinSound);
        }
    }

    public bool RemoveCurrency(int amount)
    {
        if (TotalCurrency >= amount)
        {
            TotalCurrency -= amount;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null) currencyText.text = TotalCurrency.ToString();
    }

    public void ReduceLives(int damage)
    {
        TotalLives -= damage;
        UpdateLivesUI();

        if (TotalLives <= 0)
        {
            TotalLives = 0;
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null) livesText.text = TotalLives.ToString();
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0; 
    }

    public void LevelCompleted()
    {
        Debug.Log("YOU WIN!");
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }
        Time.timeScale = 0; 
    }

    // --- 场景控制 ---

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}