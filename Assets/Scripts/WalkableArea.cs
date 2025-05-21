// WalkableArea.cs
using UnityEngine;

public class WalkableArea : MonoBehaviour
{
    [Header("地形类型")]
    public bool isWater = false;
    public bool hasObstacle = false;

    void OnDrawGizmos()
    {
        Gizmos.color = isWater ? Color.blue : new Color(1, 0.5f, 0);
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}