using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // 定义一个结构体，用来配置每种怪物的池子
    [System.Serializable]
    public class Pool
    {
        public string tag;        // 怪物的名字 (比如 "Enemy_1")
        public GameObject prefab; // 怪物的预制体
        public int size;          // 初始数量
    }

    [Header("Pool Config")]
    public List<Pool> pools; // 在 Inspector 里配置这个列表

    // 字典：通过名字(tag) 快速找到对应的队列
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform); // 放在 ObjectPooler 下面，保持整洁
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // 根据名字 (tag) 取出对象
    public GameObject GetInstanceFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("池子里没有叫这个名字的东西: " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // 重新放回队列尾部 (循环使用)
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        // 确保拿出来的时候是关闭状态 (Spawner 会负责激活它)
        objectToSpawn.SetActive(false); 

        return objectToSpawn;
    }
    
    // 为了兼容旧代码 (如果其他脚本还在调用无参数的 GetInstanceFromPool)
    // 我们可以保留这个重载，默认返回第一个池子的对象
    public GameObject GetInstanceFromPool()
    {
        return GetInstanceFromPool(pools[0].tag);
    }
    
    // 兼容 ReturnToPool (其实不需要做任何操作，只需要 Deactivate 即可)
    public static void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}