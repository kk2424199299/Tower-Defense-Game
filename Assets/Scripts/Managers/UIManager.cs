using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    public GameObject turretShopPanel; // 商店面板
    public GameObject nodeUIPanel;     // 升级面板
    public GameObject achievementPanel; // 【新增】成就面板

    private void Update()
    {
        // 快捷键测试：按 A 键开关成就面板 (可选)
        if (Input.GetKeyDown(KeyCode.A))
        {
            ToggleAchievementPanel();
        }

        // 快捷键：空格暂停
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale > 0) PauseGame();
            else SetSpeed1x();
        }
    }

    // --- 成就面板控制 ---
    
    // 打开/关闭 切换
    public void ToggleAchievementPanel()
    {
        if (achievementPanel != null)
        {
            bool isActive = achievementPanel.activeSelf;
            achievementPanel.SetActive(!isActive);
        }
    }

    // 专门给关闭按钮用的方法
    public void CloseAchievementPanel()
    {
        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false);
        }
    }

    public void OpenAchievementPanel()
    {
        if (achievementPanel != null)
        {
            achievementPanel.SetActive(true);
        }
    }

    // --- 倍速控制 ---
    public void SetSpeed1x()
    {
        Time.timeScale = 1f;
    }

    public void SetSpeed2x()
    {
        Time.timeScale = 2f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // --- 其他面板 ---
    public void CloseTurretShopPanel()
    {
        if (turretShopPanel != null) turretShopPanel.SetActive(false);
    }
    
    public void ShowTurretShopPanel()
    {
        if (turretShopPanel != null) turretShopPanel.SetActive(true);
    }
}