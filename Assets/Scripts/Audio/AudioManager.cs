using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("音频源")]
    public AudioSource bgmSource; // 专门放背景音乐 (循环)
    public AudioSource sfxSource; // 专门放音效 (不循环，短促)

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 自动查找组件 (防止你忘了拖)
        // 注意：这里假设你在这个物体上挂了两个 AudioSource
        // 为了安全起见，建议你在 Inspector 里手动拖进去，代码里只做备用检查
    }

    private void Start()
    {
        // 确保 BGM 开始播放
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    // --- BGM 控制 ---
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    // 切换背景音乐 (比如打 Boss 时换歌)
    public void PlayMusic(AudioClip clip)
    {
        if (bgmSource.clip == clip) return; // 如果是同一首就不切了

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // --- SFX (音效) 控制 ---
    
    // 调节音效总音量
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }

    // 播放一次性音效 (比如射击、金币)
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            // PlayOneShot 允许声音重叠 (比如机关枪连射)
            sfxSource.PlayOneShot(clip);
        }
    }
}