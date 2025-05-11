using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    private bool isInit;
    public Vector2 targetPosition1; // 目标位置（使用 Vector2）
    public Vector2 targetPosition2;
    public float speed = 500.0f; // 移动速度
    public float returnSpeed = 500.0f; // 返回速度
    public float collisionDistance = 1.0f; // 撞击距离
    public bool isFur;//是否有两个目标
    public bool isDou;
    private Vector2 targetPosition;

    public GameObject contorller;
    public int id;
    public bool ispla;
    private gamecontroller con;

    private Vector2 startPosition; // 起始位置（使用 Vector2）
    public bool isReturning = false; // 是否正在返回
    private bool sec = false;//是否是第二次撞击
    private bool sec1 = false;

    void Start()
    {
        targetPosition = targetPosition1;
        startPosition = transform.position; // 保存起始位置
        con = contorller.GetComponent<gamecontroller>();
        isInit = true;
    }

    void Update()
    {
        if(!isInit)
        {
            targetPosition = targetPosition1;
            startPosition = transform.position;
            isInit = true;
        }
        if (!isFur)
        {
            con.DefMove(ispla, id);
        }
        else
        {
            if (!sec)
                con.DefMove(ispla, id - 1);
            else
                con.DefMove(ispla, id + 1);
        }
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
                targetPosition = targetPosition2;
                isReturning = false;
            }
            else if (transform.position.y == startPosition.y)
            {
                isReturning = false;
                targetPosition = targetPosition1;
                sec = false;
                if (!isDou || sec1)
                {
                    enabled = false;
                    con.enabled = true;
                }
                else sec1 = true;
                isInit = false;
            }
        }
    }
}