using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DelEvent : MonoBehaviour
{
    public GameObject controller;
    public float duration = 0.5f; // 消失持续时间
    private SpriteRenderer spriteRenderer; // SpriteRenderer 引用
    private gamecontroller con;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取 SpriteRenderer 组件
        if(spriteRenderer==null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        StartCoroutine(FadeOut()); // 启动协程
        con = controller.GetComponent<gamecontroller>();
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < duration)
        {
            // 计算透明度
            float alpha = 1.0f - (elapsedTime / duration);
            color.a = alpha;
            spriteRenderer.color = color;
            elapsedTime += Time.deltaTime; // 累计时间
            yield return null; // 等待下一帧
        }
        // 动画完成后，销毁对象
        Destroy(gameObject);
        con.enabled = true;
    }
}
