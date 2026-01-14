using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Currency")]
    [SerializeField] private int startingCurrency = 100;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Lives")]
    [SerializeField] private int startingLives = 10;
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    public int TotalCurrency { get; set; }
    public int TotalLives { get; set; }

    private void Start()
    {
        // 【关键修复】强制恢复游戏时间，防止上一局结束后时间停留在 0
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
        currencyText.text = TotalCurrency.ToString();
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
        livesText.text = TotalLives.ToString();
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

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}