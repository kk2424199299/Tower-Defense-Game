using UnityEngine;

// 这是一个“泛型”类，意思是它可以变成任何类型的单例
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    
    // 公开的访问点，谁想找在这个单例，就调用 Singleton.Instance
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 尝试在场景里找
                instance = FindObjectOfType<T>();
                
                // 如果场景里没有，就自动创建一个
                if (instance == null)
                {
                    GameObject newInstance = new GameObject();
                    instance = newInstance.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        instance = this as T;
    }
}