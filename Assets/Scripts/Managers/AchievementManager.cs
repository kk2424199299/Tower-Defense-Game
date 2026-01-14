using UnityEngine;
using System; 
using System.Collections.Generic; 

public class AchievementManager : Singleton<AchievementManager>
{
    [Header("核心配置")]
    public GameObject cardPrefab;      
    public Transform contentContainer; 
    
    [Header("任务数据")]
    public List<Achievement> achievementsList; 

    // 【新增】一个通知事件：告诉所有UI“数据变了，快刷新！”
    public static event Action OnProgressUpdated;

    private void Start()
    {
        // 1. 游戏开始，先把所有任务进度归零
        foreach (var achievement in achievementsList)
        {
            achievement.ResetProgress();
        }

        GenerateCards();
    }

    // 【关键】经理始终开启监听，不管 UI 是否打开
    private void OnEnable()
    {
        Enemy.OnEnemyKilled += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= HandleEnemyDeath;
    }

    // 处理杀怪
    private void HandleEnemyDeath()
    {
        bool anyChanged = false;

        // 遍历所有任务，给没完成的任务 +1
        foreach (var achievement in achievementsList)
        {
            if (!achievement.isClaimed && achievement.currentProgress < achievement.TargetCount)
            {
                achievement.currentProgress++;
                anyChanged = true;
            }
        }

        // 如果有数据变化，大喊一声“刷新UI！”
        if (anyChanged)
        {
            OnProgressUpdated?.Invoke();
        }
    }

    private void GenerateCards()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Achievement data in achievementsList)
        {
            GameObject newCard = Instantiate(cardPrefab, contentContainer);
            AchievementCard cardScript = newCard.GetComponent<AchievementCard>();
            
            if (cardScript != null)
            {
                cardScript.Setup(data);
            }
        }
    }
}