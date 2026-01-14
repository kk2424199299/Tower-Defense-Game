using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    public string Title;       
    public int TargetCount;    
    public int Reward;         
    public Sprite Icon;        

    // 【新增】运行时数据 (这是临时的，游戏重开会重置)
    // 加上 [System.NonSerialized] 防止退出游戏后 Unity 还死记着上次的进度
    [System.NonSerialized] public int currentProgress;
    [System.NonSerialized] public bool isClaimed;

    // 重置进度的方法
    public void ResetProgress()
    {
        currentProgress = 0;
        isClaimed = false;
    }
}