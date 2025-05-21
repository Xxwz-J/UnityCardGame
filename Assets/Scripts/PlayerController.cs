// PlayerController.cs
using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 3f;
    public int moveRange = 3; // 基于节点数量的移动范围

    [Header("视觉反馈")]
    public GameObject[] stepIcons; // 不同步数对应的图标预制体（0:箭头,1:脚印...）

    private Vector3 currentPosition;
    private int currentStep;

    void Start()
    {
        currentPosition = GameObject.Find("白色房屋").transform.position + Vector3.up * 0.5f;
        transform.position = currentPosition;
        UpdateMovableArea();
    }

    void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, currentPosition) < 0.1f)
            ShowMovableArea();
    }

    private void ShowMovableArea()
    {
        throw new NotImplementedException();
    }

    void UpdateMovableArea()
    {
        // 根据当前地形生成可移动点
        Collider[] nodes = Physics.OverlapSphere(currentPosition, moveRange, LayerMask.GetMask("Walkable"));

        foreach (Collider node in nodes)
        {
            StartCoroutine(FlashIcon(node.transform.position));
        }
    }

    IEnumerator FlashIcon(Vector3 pos)
    {
        GameObject icon = Instantiate(stepIcons[currentStep % stepIcons.Length],
                                     pos, Quaternion.identity);
        float alpha = 0;

        while (true)
        {
            alpha = Mathf.PingPong(Time.time * 2, 1);
            icon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }

    public void MoveTo(Vector3 target)
    {
        StartCoroutine(MoveCoroutine(target));
    }

    IEnumerator MoveCoroutine(Vector3 target)
    {
        currentStep++;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        currentPosition = target;
        UpdateMovableArea();
    }
}