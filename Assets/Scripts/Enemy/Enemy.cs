using UnityEngine;
using System; 

public class Enemy : MonoBehaviour
{
    // 全局人口计数器
    public static int TotalEnemiesAlive = 0;

    public float MoveSpeed = 2f; 
    
    // 【新增】这个怪物走到终点会扣多少血？默认扣1点
    public int DamageToPlayer = 1; 

    public static event Action OnEnemyKilled; 

    public EnemyHealth EnemyHealth { get; private set; }

    private Waypoint _waypointScript; 
    private int _waypointIndex;

    private void OnEnable()
    {
        TotalEnemiesAlive++;
    }

    private void OnDisable()
    {
        TotalEnemiesAlive--;
    }

    private void Awake()
    {
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        _waypointScript = FindObjectOfType<Waypoint>();
        
        if (_waypointScript != null)
        {
            transform.position = _waypointScript.GetWaypointPosition(0);
        }
    }

    private void Update()
    {
        if (_waypointScript == null) return;
        Move();
    }

    private void Move()
    {
        Vector3 targetPosition = _waypointScript.GetWaypointPosition(_waypointIndex);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (_waypointIndex < _waypointScript.Points.Length - 1)
            {
                _waypointIndex++;
            }
            else
            {
                // --- 到达终点逻辑 ---
                
                // 【核心修复】这里不再是注释了，真正调用扣血！
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.ReduceLives(DamageToPlayer);
                }

                // 销毁自己
                Destroy(gameObject);
            }
        }
    }

    public void ResetEnemy()
    {
        _waypointIndex = 0;
        
        if (_waypointScript == null) _waypointScript = FindObjectOfType<Waypoint>();

        if (_waypointScript != null)
        {
            transform.position = _waypointScript.GetWaypointPosition(0);
        }

        if (EnemyHealth != null)
        {
            EnemyHealth.ResetHealth();
        }
    }

    public void ResetEnemyAnimation()
    {
        // 留空防止报错
    }

    public static void TriggerDeathEvent()
    {
        OnEnemyKilled?.Invoke();
    }
}