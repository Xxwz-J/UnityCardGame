using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class BackgroundMovement : MonoBehaviour
{
    public static BackgroundMovement Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("节点移动参数")]
    public float moveSpeed = 4f;                // 移动速度
    public float nodeSnapDistance = 0.5f;       // 节点吸附距离
    public LayerMask nodeLayer;                 // 节点层级

    [Header("视觉反馈")]
    public GameObject validStepEffect;          // 有效移动特效
    public GameObject invalidStepEffect;        // 无效点击特效
    public AnimationCurve flashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Transform currentNode;              // 当前所在节点
    private Transform targetNode;               // 目标节点
    public bool isMoving = false;              // 移动状态标识

    void Start()
    {
        // 初始化时自动寻找最近节点
        Collider[] nodes = Physics.OverlapSphere(transform.position, 10f, nodeLayer);
        float minDistance = Mathf.Infinity;

        foreach (Collider node in nodes)
        {
            float distance = Vector3.Distance(transform.position, node.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentNode = node.transform;
            }
        }

        // 将角色吸附到最近节点
        if (currentNode != null)
            transform.position = currentNode.position + Vector3.up * 0.3f;
    }

    void Update()
    {
        HandleMovementInput();
        SmoothMovement();
    }

    void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, nodeLayer))
            {
                Node node = hit.collider.GetComponent<Node>();
                if (node != null && IsNodeConnected(node.transform))
                {
                    StartCoroutine(MoveToNode(node.transform));
                }
                else
                {
                    ShowInvalidClickEffect(hit.point);
                }
            }
        }
    }

    bool IsNodeConnected(Transform target)
    {
        // 获取当前节点的连接关系
        Node currentNodeScript = currentNode.GetComponent<Node>();
        return currentNodeScript.connectedNodes.Contains(target);
    }

    IEnumerator MoveToNode(Transform target)
    {
        isMoving = true;
        targetNode = target;

        // 移动前准备
        Vector3 startPos = transform.position;
        Vector3 endPos = targetNode.position + Vector3.up * 0.3f;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;

        // 移动过程
        while (Vector3.Distance(transform.position, endPos) > nodeSnapDistance)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            yield return null;
        }

        // 移动完成
        currentNode = targetNode;
        ShowValidStepEffect(endPos);
        isMoving = false;
    }

    void SmoothMovement()
    {
        // 自动吸附到最近节点
        if (!isMoving)
        {
            Collider[] nodes = Physics.OverlapSphere(transform.position, nodeSnapDistance, nodeLayer);
            if (nodes.Length > 0)
            {
                transform.position = nodes[0].transform.position + Vector3.up * 0.3f;
            }
        }
    }

    void ShowValidStepEffect(Vector3 position)
    {
        StartCoroutine(FlashEffect(validStepEffect, position));
    }

    void ShowInvalidClickEffect(Vector3 position)
    {
        StartCoroutine(FlashEffect(invalidStepEffect, position));
    }

    IEnumerator FlashEffect(GameObject effectPrefab, Vector3 position)
    {
        GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);
        SpriteRenderer sr = effect.GetComponent<SpriteRenderer>();

        float duration = 1f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = flashCurve.Evaluate(t / duration);
            if (sr != null) sr.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        Destroy(effect);
    }
}