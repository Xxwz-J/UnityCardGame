using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [Header("节点连接")]
    public List<Transform> connectedNodes = new List<Transform>();  // 可连接的节点

    [Header("视觉设置")]
    public Color highlightColor = Color.yellow;    // 悬停高亮颜色
    public float glowIntensity = 1.2f;             // 发光强度

    private Material originalMaterial;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        AutoDetectConnections();
    }

    void AutoDetectConnections()
    {
        // 自动检测2米范围内的相邻节点
        Collider[] nodes = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider col in nodes)
        {
            if (col.CompareTag("Node") && col.transform != transform)
            {
                connectedNodes.Add(col.transform);
            }
        }
    }

    void OnMouseEnter()
    {
        if (!BackgroundMovement.Instance.isMoving)
        {
            GetComponent<Renderer>().material.color = highlightColor * glowIntensity;
        }
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalMaterial.color;
    }

    void OnDrawGizmosSelected()
    {
        // 绘制连接线
        Gizmos.color = Color.cyan;
        foreach (Transform node in connectedNodes)
        {
            if (node != null)
                Gizmos.DrawLine(transform.position, node.position);
        }
    }
}