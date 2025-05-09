using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public Vector2 targetPosition; // 目标位置（使用 Vector2）
    public Vector2 targetPosition1;
    public float speed = 500.0f; // 移动速度
    public float returnSpeed = 500.0f; // 返回速度
    public float collisionDistance = 1.0f; // 撞击距离
    public bool isFur;//是否有两个目标

    private Vector2 startPosition; // 起始位置（使用 Vector2）
    public bool isReturning = false; // 是否正在返回
    private bool sec = false;

    void Start()
    {
        startPosition = transform.position; // 保存起始位置
    }

    void Update()
    {
        if (!isReturning)
        {
            // 移动到目标位置
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;

            // 检测是否到达目标位置
            if (Vector2.Distance(transform.position, targetPosition) < collisionDistance)
            {
                isReturning = true; // 开始返回
            }
        }
        else
        {
            // 返回起始位置
            transform.position = Vector2.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
            if (transform.position.y == startPosition.y && isFur && !sec)
            {
                sec = true;
                targetPosition = targetPosition1;
                isReturning = false;
            }
            else if (transform.position.y == startPosition.y)
                enabled = false;
        }
    }
}