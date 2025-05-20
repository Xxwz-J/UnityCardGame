// 节点点击脚本 SimpleNode.cs
using UnityEngine;

public class SimpleNode : MonoBehaviour
{
    public string nodeName; // 对应地图上的文字（如Grano/Dombio）

    void OnMouseDown()
    {
        PathDrawer.Instance.AddPoint(transform.position);
    }
}

// 路径绘制管理器

public class PathDrawer : MonoBehaviour
{
    public static PathDrawer Instance;
    public LineRenderer line;
    public Color lineColor = Color.red;

    void Awake()
    {
        Instance = this;
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = line.endWidth = 0.3f;
        line.material.color = lineColor;
    }

    public void AddPoint(Vector3 pos)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, pos + Vector3.up * 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // 按C键清除路径
            line.positionCount = 0;
    }
}