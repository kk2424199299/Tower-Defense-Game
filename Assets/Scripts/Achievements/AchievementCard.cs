using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementCard : MonoBehaviour
{
    [Header("UI 组件")]
    public Image IconImage;           
    public TextMeshProUGUI TitleTxt;  
    public TextMeshProUGUI ProgressTxt; 
    public TextMeshProUGUI RewardTxt;   
    public Button ClaimButton;          
    public TextMeshProUGUI ClaimBtnText; 

    [Header("Audio")]
    // 【关键】这里用来放领奖音效
    public AudioClip claimSound;

    private Achievement _data;          

    public void Setup(Achievement data)
    {
        _data = data;
        IconImage.sprite = _data.Icon;
        TitleTxt.text = _data.Title;
        RewardTxt.text = "$" + _data.Reward;

        UpdateUI(); 
    }

    private void OnEnable()
    {
        UpdateUI();
        AchievementManager.OnProgressUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        AchievementManager.OnProgressUpdated -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (_data == null) return;

        ProgressTxt.text = $"{_data.currentProgress} / {_data.TargetCount}";

        if (_data.isClaimed)
        {
            ClaimButton.interactable = false;
            ProgressTxt.color = Color.gray;
            if (ClaimBtnText != null) ClaimBtnText.text = "CLAIMED";
        }
        else if (_data.currentProgress >= _data.TargetCount)
        {
            ClaimButton.interactable = true;
            ProgressTxt.color = Color.green;
            if (ClaimBtnText != null) ClaimBtnText.text = "CLAIM";
        }
        else
        {
            ClaimButton.interactable = false;
            ProgressTxt.color = Color.white;
            if (ClaimBtnText != null) ClaimBtnText.text = "CLAIM";
        }
    }

    // 点击按钮时会调用这个方法
    public void ClaimReward()
    {
        // 只有没领过，且进度达标了才能领
        if (!_data.isClaimed && _data.currentProgress >= _data.TargetCount)
        {
            _data.isClaimed = true;

            // 加钱
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.AddCurrency(_data.Reward);
            }

            // 【核心逻辑】播放领奖音效
            if (AudioManager.Instance != null && claimSound != null)
            {
                AudioManager.Instance.PlaySFX(claimSound);
            }

            UpdateUI();
        }
    }
}