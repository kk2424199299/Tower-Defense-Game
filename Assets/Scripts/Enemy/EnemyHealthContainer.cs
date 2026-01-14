using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthContainer : MonoBehaviour
{
    [SerializeField] private Image fillAmountImage;

    // 这是一个公开的“接口”，让外面的脚本能访问到私有的图片
    public Image FillAmountImage => fillAmountImage;
}