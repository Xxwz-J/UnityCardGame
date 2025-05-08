using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // 震动持续时间
    public float shakeMagnitude = 0.1f; // 震动强度
    private Vector3 initialPosition; // 初始位置
    private float elapsedTime; // 已过去的时间

    void Start()
    {
        initialPosition = transform.position; // 保存初始位置
    }

    void Update()
    {
        if (elapsedTime < shakeDuration)
        {
            // 计算新的位置
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            transform.position = initialPosition + new Vector3(x, y, 0);
            elapsedTime += Time.deltaTime; // 累计时间
        }
        else
        {
            // 恢复初始位置
            transform.position = initialPosition;
            this.enabled = false;
        }
    }
}
