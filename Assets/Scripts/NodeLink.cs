// NodeLink.cs 节点数据组件
using UnityEngine;
using System.Collections.Generic;

public class NodeLink : MonoBehaviour
{
    [Header("节点标识")]
    public string nodeID = "Grano";

    [Header("连接设置")]
    public Color lineColor = Color.cyan; // 按地图元素类型设置颜色
    [Range(0.1f, 2f)]
    public float widthMultiplier = 1f;   // 节点专属线宽系数

    public List<Transform> connectedNodes = new List<Transform>();
}
