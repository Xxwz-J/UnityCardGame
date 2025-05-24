using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // 保持原有public字段不变
    public Text nameText;
    public Text attackText;
    public Text healthText;
    public Text sacrificeText;
    public Image background;
    public GameObject StampPool;
    public GameObject StampPrefab;
    public Card card;
    public List<GameObject> stamps;
    void Start()
    {
        ShowCard();
    }

    public void ShowCard()
    {
        ClearPool();
        // 防御层1：核心对象空引用检查
        if (card == null)
        {
            Debug.LogWarning("Card reference is not set in CardDisplay!");
            return;
        }

        // 防御层2：UI组件安全访问
        SafeSetText(nameText, card.cardName);

        if (card is MonsterCard monster)
        {
            // 防御层3：数值显示安全访问
            SafeSetText(attackText, monster.attack.ToString());
            SafeSetText(healthText, monster.health.ToString());
            SafeSetText(sacrificeText, monster.sacrifice.ToString());

            // 防御层4：图片加载安全机制
            string imagePath = $"CardImages/{monster.cardName}";
            Sprite cardSprite = Resources.Load<Sprite>(imagePath);

            if (background != null)
            {
                if (cardSprite != null)
                {
                    background.sprite = cardSprite;
                    background.color = Color.white;
                }
                else
                {
                    Debug.LogWarning($"卡牌图片缺失: {monster.cardName}");
                    background.color = Color.clear;
                }
            }

            // 防御层5：印记系统安全生成
            if (StampPool != null && StampPrefab != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (monster.stamps[i] == Stamp.NullStamp) continue;

                    GameObject stamp = Instantiate(StampPrefab, StampPool.transform);
                    stamps.Add(stamp);
                    string stampPath = $"StampImages/{monster.stamps[i]}";
                    Image stampImage = stamp.GetComponent<Image>();

                    if (stampImage != null)
                    {
                        Sprite s = Resources.Load<Sprite>(stampPath);
                        if (s != null) stampImage.sprite = s;
                    }
                }
            }
        }
    }

    // 新增的安全文本设置方法
    private void SafeSetText(Text textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
        else
        {
            Debug.LogWarning($"未找到文本组件: {textComponent?.gameObject.name}");
        }
    }

    private void ClearPool()
    {
        foreach (var stamp in stamps)
        {
            Destroy(stamp);
        }
        stamps.Clear();
    }

}