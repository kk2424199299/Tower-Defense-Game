using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float moveDistance = 1.5f;   // 左右最大移动距离
    public float moveSpeed = 0.3f;       // 移动速度

    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        if (movingRight)
        {
            transform.position += Vector3.right * step;

            if (transform.position.x >= startPos.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position -= Vector3.right * step;

            if (transform.position.x <= startPos.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }
}
