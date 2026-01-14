using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementCard : MonoBehaviour
{
    [Header("UI 组件")]
    public Image IconImage;           
    public TextMeshProUGUI TitleTxt;  
    public TextMeshProUGUI ProgressTxt; 
    public TextMeshProUGUI RewardTxt;   // 这是显示 "$100" 的
    public Button ClaimButton;          // 这是按钮本身
    
    // 【新增】专门用来控制按钮上的 "CLAIM" 文字
    public TextMeshProUGUI ClaimBtnText; 

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
            
            // 【修改】直接使用变量，不再瞎找了
            if (ClaimBtnText != null) ClaimBtnText.text = "CLAIMED";
        }
        else if (_data.currentProgress >= _data.TargetCount)
        {
            ClaimButton.interactable = true;
            ProgressTxt.color = Color.green;
            
            // 【修改】
            if (ClaimBtnText != null) ClaimBtnText.text = "CLAIM";
        }
        else
        {
            ClaimButton.interactable = false;
            ProgressTxt.color = Color.white;
            
            // 【修改】未完成时也显示 CLAIM
            if (ClaimBtnText != null) ClaimBtnText.text = "CLAIM";
        }
    }

    public void ClaimReward()
    {
        if (!_data.isClaimed && _data.currentProgress >= _data.TargetCount)
        {
            _data.isClaimed = true;

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.AddCurrency(_data.Reward);
            }

            UpdateUI();
        }
    }
}