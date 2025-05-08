using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public Vector3 target; // 目标对象
    public GameObject target1;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private Rigidbody rb; // 当前对象的 Rigidbody 引用

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        rb = GetComponent<Rigidbody>(); // 获取 Rigidbody 组件
    }

    void Update()
    {
        // 每帧向目标对象移动
        Vector3 direction = (target - transform.position).normalized;
        rb.AddForce(direction * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 检测到碰撞时触发
        if (collision.gameObject.transform.position == target)
        {
            Debug.Log("撞击成功！");
            // 可以在这里添加撞击后的效果，如触发动画、播放音效等
            if(target1!=null)
            {
                Shake s = target1.GetComponent<Shake>();
                s.enabled = true;
            }
            SmoothReturn();
            this.enabled = false;
        }
    }
    private System.Collections.IEnumerator SmoothReturn()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }
}
