using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveE : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector3 position;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            position,
            800.0f * Time.deltaTime);
        if (rectTransform.anchoredPosition.x==  position.x)
            enabled = false;
    }
}
