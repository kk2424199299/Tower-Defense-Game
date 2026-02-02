using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI 面板引用")]
    public GameObject settingsPanel;

    // --- 场景控制 ---
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1"); 
    }

    public void QuitGame()
    {
        Debug.Log("正在退出游戏...");
        Application.Quit();
    }

    // --- 设置面板控制 ---
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // --- 声音与画面设置 ---

    // 1. 背景音乐音量 (绑定到 Music Slider)
    public void SetBGMVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(volume);
        }
    }

    // 2. 音效音量 (绑定到 SFX Slider) 【新增】
    public void SetSFXVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }
    }

    // 3. 全屏开关
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}