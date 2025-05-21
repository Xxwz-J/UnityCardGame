// NodeLinkEditor.cs 定制Inspector
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeLink))]
public class NodeLinkEditor : Editor
{
    void OnSceneGUI()
    {
        NodeLink node = (NodeLink)target;

        // 添加连接按钮
        Handles.BeginGUI();
        if (GUILayout.Button("+ 快速连接", GUILayout.Width(100)))
        {
            // 打开节点选择窗口
            ShowNodeSelection(node);
        }
        Handles.EndGUI();

        // 拖动端点功能
        for (int i = 0; i < node.connectedNodes.Count; i++)
        {
            if (node.connectedNodes[i] == null) continue;
            Vector3 newPos = Handles.PositionHandle(
                node.connectedNodes[i].position + Vector3.up * 2f,
                Quaternion.identity
            );
            if (newPos != node.connectedNodes[i].position)
            {
                Undo.RecordObject(node.connectedNodes[i], "Move Connected Node");
                node.connectedNodes[i].position = newPos - Vector3.up * 2f;
            }
        }
    }

    void ShowNodeSelection(NodeLink source)
    {
        GenericMenu menu = new GenericMenu();
        foreach (Transform node in source.transform.parent)
        {
            if (node == source.transform) continue;
            menu.AddItem(
                new GUIContent(node.name),
                false,
                () => { source.connectedNodes.Add(node); }
            );
        }
        menu.ShowAsContext();
    }
}
#endif