using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObl : MonoBehaviour, IPointerClickHandler
{
    public Sprite patternSprite; // 在Inspector中分配图案Sprite
    public bool isSel = false;
    private playhand pla;
    private gamecontroller con;
    public void Awake()
    {
        pla = GetComponent<playhand>();
        con = pla.controller.GetComponent<gamecontroller>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pla.isPlaced)
        {
            if (isSel)
            {
                // 获取当前对象的 SpriteRenderer 组件
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

                // 检查是否存在 SpriteRenderer 组件
                if (spriteRenderer != null)
                {
                    // 删除 SpriteRenderer 组件
                    Destroy(spriteRenderer);
                }
                isSel = false;
                con.isUsed[pla.idoftarget] = false;
            }
            else
            {
                // 添加SpriteRenderer组件
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(transform);
                // 设置图案Sprite
                spriteRenderer.sprite = patternSprite;

                // 调整渲染顺序（确保图案在正确层级）
                spriteRenderer.sortingOrder = 1;

                // 调整颜色和透明度
                spriteRenderer.color = new Color(1, 1, 1, 0.8f); // 80%不透明度
                isSel = true;
                con.isUsed[pla.idoftarget] = true;
            }
        }
    }
}
