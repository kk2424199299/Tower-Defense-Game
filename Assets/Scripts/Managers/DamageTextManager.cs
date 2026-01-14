using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 它继承自 Singleton，所以它自动变成了一个全局唯一的管理器
public class DamageTextManager : Singleton<DamageTextManager>
{
    // 用来管理对象池的引用
    public ObjectPooler Pooler { get; set; }

    private void Start()
    {
        // 获取挂在同一个物体上的 ObjectPooler 组件
        Pooler = GetComponent<ObjectPooler>();
    }
}