using System.Collections;
using System.Collections.Generic;
using TMPro; // 引用 TextMeshPro 命名空间
using UnityEngine;

public class DamageText : MonoBehaviour
{
    // 获取子物体里的文字组件
    public TextMeshProUGUI DmgText => GetComponentInChildren<TextMeshProUGUI>();

    // 这个方法会在动画结束时被调用，用来回收自己
    public void ReturnTextToPool()
    {
        transform.SetParent(null); // 断开父子关系
        ObjectPooler.ReturnToPool(gameObject); // 回收进池子
    }
}