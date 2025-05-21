#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#endif

public class NodeLinkManager : MonoBehaviour
{
    [Header("连线样式")]
    public float lineWidth = 3.5f;        // 基础线宽
    public Gradient lineColorGradient;    // 渐变颜色
    public Texture2D lineTexture;         // 路径纹理

    private List<LineRenderer> activeLines = new List<LineRenderer>();

    void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += UpdateLines;
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= UpdateLines;
#endif
    }

    void UpdateLines()
    {
        ClearExistingLines();

        foreach (Transform node in transform)
        {
            NodeLink link = node.GetComponent<NodeLink>();
            if (!link) continue;

            foreach (Transform target in link.connectedNodes)
            {
                NodeLink targetLink = target.GetComponent<NodeLink>();
                if (!targetLink) continue;

                CreateLineBetween(
                    node.position + Vector3.up * 1.5f,
                    target.position + Vector3.up * 1.5f,
                    link.lineColor != Color.clear ? link.lineColor : Color.cyan, // 默认蓝绿色
                    targetLink.lineColor != Color.clear ? targetLink.lineColor : Color.cyan
                );
            }
        }
    }

    void CreateLineBetween(Vector3 start, Vector3 end, Color startColor, Color endColor)
    {
        GameObject lineObj = new GameObject("EditorLine");
        lineObj.transform.SetParent(transform);

        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        line.startWidth = lineWidth;
        line.endWidth = lineWidth * 0.8f; // 末端略微收窄
        line.colorGradient = CreateGradient(startColor, endColor);
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.mainTexture = lineTexture;
        line.textureMode = LineTextureMode.Tile;

        activeLines.Add(line);
    }

    Gradient CreateGradient(Color start, Color end)
    {
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(start, 0),
                new GradientColorKey(end, 1)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1, 0),
                new GradientAlphaKey(1, 1)
            }
        );
        return grad;
    }

    void ClearExistingLines()
    {
        foreach (LineRenderer line in activeLines)
            DestroyImmediate(line.gameObject);
        activeLines.Clear();
    }
    // 在NodeLinkManager中添加流动效果：
    void Update()
    {
        foreach (LineRenderer line in activeLines)
        {
            line.material.mainTextureOffset += Vector2.right * Time.deltaTime;
        }
    }
}