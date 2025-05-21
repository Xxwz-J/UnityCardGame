using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Text nameText;
    public Text attackText;
    public Text healthText;
    public Text sacrificeText;
    public Image background;
    public GameObject StampPool;
    public GameObject StampPrefab;
    public Card card;

    // Start is called before the first frame update
    void Start()
    {
        ShowCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCard()
    {
        nameText.text=card.cardName;

        if(card is MonsterCard)
        {
            var monster =  card as MonsterCard;
            attackText.text = monster.attack.ToString();
            healthText.text = monster.health.ToString();
            sacrificeText.text = monster .sacrifice.ToString();
            string path = $"CardImages/{monster.cardName}";

            string imagePath = $"CardImages/{monster.cardName}";
            //string imagePath = $"CardImages/松鼠";
            Sprite cardSprite = Resources.Load<Sprite>(imagePath);

            if (cardSprite != null)
            {
                background.sprite = cardSprite;
                background.color = Color.white; // 确保Image组件启用
            }
            else
            {
                Debug.LogWarning($"卡牌图片缺失: {monster.cardName}");
                background.color = Color.clear; // 隐藏无图片状态
            }

            for (int i = 0; i < 3; i++) {
                MonsterCard monsterCard = (MonsterCard)card;
                if (monsterCard.stamps[i] == Stamp.NullStamp) break;
                GameObject stamp = Instantiate(StampPrefab, StampPool.transform);
                string stampPath = $"StampImages/{monsterCard.stamps[i]}";
                StampPrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>(stampPath);
            }

            // Text.gameObject.SetActive(false)-- hide
        }
        //还需更新印记显示
    }
}
