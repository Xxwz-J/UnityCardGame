using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveE : MonoBehaviour
{
    public GameObject controller;
    private gamecontroller con;
    private RectTransform rectTransform;
    public Vector3 position;
    public bool begin;
    private bool del = false;
    private void Start()
    {
        con = controller.GetComponent<gamecontroller>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if(begin)
        {
            DelEvent d = gameObject.GetComponent<DelEvent>();
            if (d.enabled)
            {
                d.enabled = false;
                d.StopAllCoroutines();
                del = true;
            }
            begin = false;
        }
        if (transform.rotation.z != 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 9f);
        }
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            position,
            1800.0f * Time.deltaTime);
        if (rectTransform.anchoredPosition.x == position.x && rectTransform.anchoredPosition.y == position.y&& transform.rotation== Quaternion.identity)
        {
            enabled = false;
            con.mov = false;
            if(del)
            {
                DelEvent d = gameObject.GetComponent<DelEvent>();
                d.enabled = true;
                d.StartExternalCoroutine();
            }
            if (con.reend)
            {
                con.re = true;
            }
        }
    }
}
