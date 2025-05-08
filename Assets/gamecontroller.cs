using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class gamecontroller : MonoBehaviour
{
    public GameObject module;
    public bool isplayerbout; //是否是玩家回合
    public MonsterCard[] playercards = new MonsterCard[4];
    public GameObject[] showedpla = new GameObject[4];
    public MonsterCard[] allEnemy; //敌人出现顺序
    public bool[] isEmpty; //玩家方是否有出战牌
    public bool[] isUsed;//是否被选择献祭
    public Camera uiCamera;
    private int numofRound = 0;//回合数
    public MonsterCard[] firstlinecards;//第一排敌人
    public GameObject[] firshowed;
    private MonsterCard[] secondlinecards;//第二排敌人
    private GameObject[] secshowed;
    private Queue<MonsterCard> firstline;//第一列敌人
    private Queue<MonsterCard> secondline;//第二列敌人
    private Queue<MonsterCard> thirdline;//第三列敌人
    private Queue<MonsterCard> fourthline;//第四列敌人
    private bool begin;//玩家是否攻击
    private bool ready;//敌方是否攻击
    private int idofDe1; //玩家方阻挡印记拥有者id
    private int idofDe2; //敌方阻挡印记拥有者id
    private int damagePlayerReceived;
    private int damageEnemyReceived;
    private bool gameOver;
    private Vector3 v1 = new Vector3(0, 0, -100);//相机位置1
    private Vector3 v2 = new Vector3(0, -170, -100);//相机位置2
    private Vector2[] positionpla = new Vector2[4];
    public Vector2[] firstv = new Vector2[4];//第一排卡牌位置
    private Vector2[] secondv = new Vector2[4];//第二排卡牌位置
    private Vector2 size = new Vector2(107, 133);
    private CardStore data;
    // Start is called before the first frame update
    void Start()
    {
        uiCamera.transform.position = v1;
        isplayerbout = true;
        begin = false;
        ready = false;
        idofDe1 = -1;
        idofDe2 = -1;
        isEmpty = new bool[4];
        isUsed = new bool[4];
        firstlinecards = new MonsterCard[4];
        secondlinecards = new MonsterCard[4];
        for (int i = 0; i < 4; i++)
        {
            isEmpty[i] = true;
            playercards[i] = null;
            showedpla[i] = null;
            isUsed[i] = false;
            firstlinecards[i] = null;
            secondlinecards[i] = null;
        }
        damageEnemyReceived = 0;
        damagePlayerReceived = 0;
        gameOver = false;
        positionpla[0] = new Vector2(-268, 103);
        positionpla[1] = new Vector2(-118, 103);
        positionpla[2] = new Vector2(32, 103);
        positionpla[3] = new Vector2(182, 103);
        firstv[0] = new Vector2(-268, -70);
        firstv[1] = new Vector2(-118, -70);
        firstv[2] = new Vector2(32, -70);
        firstv[3] = new Vector2(182, -70);
        secondv[0] = new Vector2(-268, 83);
        secondv[1] = new Vector2(-118, 83);
        secondv[2] = new Vector2(32, 83);
        secondv[3] = new Vector2(182, 83);
        data = GetComponent<CardStore>();
        //InitEnemy();
    }
    // Update is called once per frame
    void Update()
    {
        if (isplayerbout)
        {
            if (Input.GetKey(KeyCode.W))
            {
                ChangeCam(1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                ChangeCam(0);
            }
        }
        if (isplayerbout == false && begin == false)
        {
            PlayerAttack();
        }
        else if (isplayerbout == false && ready == false)
        {
            EnemyAttack();
        }
        else
        {
            isplayerbout = true;
            begin = false;
            ready = false;
        }
    }
    //卡牌显示
    private void Buildcard(int line,MonsterCard card,int id)
    {
        if (card != null)
        {
            GameObject square = Instantiate(module);
            square.GetComponent<CardDisplay>().card = card;
            square.transform.SetParent(FindObjectOfType<Canvas>().transform);

            RectTransform rectTransform = square.GetComponent<RectTransform>();
            square.transform.SetParent(transform, false);
            if (line == 1)
                rectTransform.anchoredPosition = firstv[id];
            else if (line == 2) rectTransform.anchoredPosition = secondv[id];
            rectTransform.sizeDelta = size;

            square.AddComponent<SelectObl>();
            square.AddComponent<Rigidbody>();
            Attack a = square.AddComponent<Attack>();
            a.enabled = false;
            Shake s = square.AddComponent<Shake>();
            s.enabled = false;
            DelEvent d = square.AddComponent<DelEvent>();
            d.enabled = false;
            if (line == 1)
                firshowed[id] = square;
            else if (line == 2)
                secshowed[id] = square;
            a.target = positionpla[id];
            a.target1 = showedpla[id];
        }
    }
    //敌人卡牌从第二排进入第一排
    private void CardMove(int id)
    {
        Transform t = secshowed[id].GetComponent<Transform>();
        t.position = Vector3.MoveTowards(t.position, firstv[id], 5.0f * Time.deltaTime);
        firshowed[id] = secshowed[id];
        secshowed[id] = null;
    }
    //初始加载第一排敌人
    private void InitEnemy()
    {
        Array.Copy(allEnemy, 0, firstlinecards, 0, 4);
        for(int i=0;i<4;i++)
        {
            Buildcard(1, firstlinecards[i], i);
        }
    }
    //玩家卡牌攻击阶段
    private void PlayerAttack()
    {
        AttackEvent();
        int result = damageEnemyReceived - damagePlayerReceived;
        gameOver = result > 5 || result < -5;
        if (gameOver)
            GameOver(result > 5);
        else begin = true;
    }
    //更换游戏视角
    private void ChangeCam(int idofCam)
    {
        if (idofCam == 0)
        {
            uiCamera.transform.position = v2;
        }
        if (idofCam == 1)
        {
            uiCamera.transform.position = v1;
        }
    }
    //敌人卡牌攻击阶段
    private void EnemyAttack()
    {
        for (int i = 0; i < 4; i++)
            if (firstlinecards[i] == null)
            {
                firstlinecards[i] = secondlinecards[i];
                secondlinecards[i] = null;
                CardMove(i);
            }
        AttackEvent();
        int result = damageEnemyReceived - damagePlayerReceived;
        gameOver = result > 5 || result < -5;
        if (gameOver)
            GameOver(result > 5);
        else
        {
            ready = true;
            LoadEnemy();
        }
    }
    //每回合加载敌人
    private void LoadEnemy()
    {
        numofRound++;
        if (numofRound * 4 < allEnemy.Length)
        {
            if (secondlinecards[0] == null)
            {
                if (firstline.Count == 0)
                    secondlinecards[0] = allEnemy[numofRound * 4];
                else
                {
                    secondlinecards[0] = firstline.Dequeue();
                    firstline.Enqueue(allEnemy[numofRound * 4]);
                }
            }
            else
                firstline.Enqueue(allEnemy[numofRound * 4]);

            if (secondlinecards[1] == null)
            {
                if (firstline.Count == 0)
                    secondlinecards[1] = allEnemy[numofRound * 4 + 1];
                else
                {
                    secondlinecards[1] = secondline.Dequeue();
                    firstline.Enqueue(allEnemy[numofRound * 4 + 1]);
                }
            }
            else
                firstline.Enqueue(allEnemy[numofRound * 4 + 1]);

            if (secondlinecards[2] == null)
            {
                if (firstline.Count == 0)
                    secondlinecards[2] = allEnemy[numofRound * 4 + 2];
                else
                {
                    secondlinecards[2] = thirdline.Dequeue();
                    firstline.Enqueue(allEnemy[numofRound * 4 + 2]);
                }
            }
            else
                firstline.Enqueue(allEnemy[numofRound * 4 + 2]);

            if (secondlinecards[3] == null)
            {
                if (firstline.Count == 0)
                    secondlinecards[3] = allEnemy[numofRound * 4 + 3];
                else
                {
                    secondlinecards[3] = fourthline.Dequeue();
                    firstline.Enqueue(allEnemy[numofRound * 4 + 3]);
                }
            }
            else
                firstline.Enqueue(allEnemy[numofRound * 4 + 3]);
        }
    }
    //游戏结束
    private void GameOver(bool playerWin)
    {
        if(playerWin)
        {
            //胜利结果
        }
        else
        {
            //战败play
        }
    }
    //我方卡牌激活刻印
    private void Stamps1(int id)
    {
        bool isFlying = false;
        bool isFur = false;
        bool isPio = false;
        bool isGro = false;
        bool isMot = false;
        FindDefStamp();
        for (int i=0;i<3;i++)
        {
            switch (playercards[id].stamps[i])
            {
                case Stamp.Flying:
                    isFlying = true;
                    break;
                case Stamp.Furcation:
                    isFur = true;
                    break;
                case Stamp.Poison:
                    isPio = true;
                    break;
                case Stamp.Growth:
                    isGro = true;
                    GrowthStamp1(id);
                    playercards[id].stamps[i] = Stamp.NullStamp;
                    break;
                case Stamp.Motion:
                    isMot = true;
                    break;
            }
        }
        if(isFur)
        {
            if(isFlying)
            {
                damageEnemyReceived += playercards[id].attack * 2;
            }
            else if(isPio)
            {
                if (id > 0)
                {
                    if (firstlinecards[id - 1] != null)
                        firstlinecards[id - 1].health = 0;
                    else damageEnemyReceived += playercards[id].attack;
                }
                if (id < 3)
                {
                    if (firstlinecards[id + 1] != null)
                        firstlinecards[id + 1].health = 0;
                    else damageEnemyReceived += playercards[id].attack;
                }
            }
            else
            {
                if (id>0)
                {
                    AttackFront1(playercards[id], id - 1);
                }
                if(id<3)
                {
                    AttackFront1(playercards[id], id + 1);
                }
            }
        }
        else
        {
            AttackFront1(playercards[id], id);
        }
        if(isGro)
        {
            GrowthStamp1(id);
        }
        if(isMot)
        {
            if (id > 0 && playercards[id-1]==null)
            {
                playercards[id - 1] = playercards[id];
                playercards[id] = null;
            }
            else if (id < 3 && playercards[id+1]==null)
            {
                playercards[id + 1] = playercards[id];
                playercards[id] = null;
            }
        }
    }
    //敌方卡牌激活刻印
    private void Stamps2(int id)
    {
        bool isFlying = false;
        bool isFur = false;
        bool isPio = false;
        bool isGro = false;
        bool isMot = false;
        FindDefStamp();
        for (int i = 0; i < 3; i++)
        {
            switch (firstlinecards[id].stamps[i])
            {
                case Stamp.Flying:
                    isFlying = true;
                    break;
                case Stamp.Furcation:
                    isFur = true;
                    break;
                case Stamp.Poison:
                    isPio = true;
                    break;
                case Stamp.Growth:
                    isGro = true;
                    GrowthStamp2(id);
                    playercards[id].stamps[i] = Stamp.NullStamp;
                    break;
                case Stamp.Motion:
                    isMot = true;
                    break;
            }
        }
        if (isFur)
        {
            if (isFlying)
            {
                damagePlayerReceived += firstlinecards[id].attack * 2;
            }
            else if (isPio)
            {
                if (id > 0)
                {
                    if (playercards[id - 1] != null)
                        playercards[id - 1].health = 0;
                    else damagePlayerReceived += firstlinecards[id].attack;
                }
                if (id < 3)
                {
                    if (playercards[id + 1] != null)
                        playercards[id + 1].health = 0;
                    else damagePlayerReceived += firstlinecards[id].attack;
                }
            }
            else
            {
                if (id > 0)
                {
                    AttackFront2(firstlinecards[id], id - 1);
                }
                if (id < 3)
                {
                    AttackFront2(firstlinecards[id], id + 1);
                }
            }
        }
        else
        {
            AttackFront2(firstlinecards[id], id);
        }
        if (isGro)
        {
            GrowthStamp2(id);
        }
        if (isMot)
        {
            if (id > 0 && firstlinecards[id - 1] == null)
            {
                firstlinecards[id - 1] = firstlinecards[id];
                firstlinecards[id] = null;
            }
            else if (id < 3 && firstlinecards[id + 1] == null)
            {
                firstlinecards[id + 1] = firstlinecards[id];
                firstlinecards[id] = null;
            }
        }
    }
    //我方卡牌攻击前方敌人
    private void AttackFront1(MonsterCard card,int id)
    {
        if (firstlinecards[id] == null)
            if(idofDe2==-1)
                damageEnemyReceived += card.attack;
            else
            {
                firstlinecards[id] = firstlinecards[idofDe2];
                idofDe2 = id;
                AttackFront1(card, id);
            }
        else
            {
                if (card.attack > firstlinecards[id].health)
                {
                    if (card.attack - firstlinecards[id].health > secondlinecards[id].health)
                        secondlinecards[id].health = 0;
                    else secondlinecards[id].health -= (card.attack - firstlinecards[id].health);
                    firstlinecards[id].health = 0;
                }
                else firstlinecards[id].health -= card.attack;
            }
        CleanCard();
    }
    //敌方卡牌攻击前方
    private void AttackFront2(MonsterCard card, int id)
    {
        if (playercards[id] == null || playercards[id].health==0)
            if(idofDe1==-1)
                damagePlayerReceived += card.attack;
            else
            {
                playercards[id] = playercards[idofDe1];
                idofDe1 = id;
                AttackFront2(card, id);
            }
        else
            {
                if (card.attack > playercards[id].health)
                {
                    playercards[id].health = 0;
                }
                else playercards[id].health -= card.attack;
            }
        CleanCard();
    }
    //清除血量为0的卡牌
    private void CleanCard()
    {
        for(int i=0;i<4;i++)
        {
            if (playercards[i]!=null)
                if (playercards[i].health==0)
                {
                    playercards[i] = null;
                    showedpla[i].GetComponent<DelEvent>().enabled = true;
                    showedpla[i] = null;
                    //
                    if (i == idofDe1)
                        FindDefStamp();
                }
            if (firstlinecards[i]!=null)
                if (firstlinecards[i].health==0)
                {
                    firstlinecards[i] = null;
                    firshowed[i].GetComponent<DelEvent>().enabled = true;
                    firshowed[i] = null;
                    //
                    if (i == idofDe2)
                        FindDefStamp();
                }
            if (secondlinecards[i]!=null)
                if (secondlinecards[i].health==0)
                {
                    secondlinecards[i] = null;
                    secshowed[i].GetComponent<DelEvent>().enabled = true;
                    secshowed[i] = null;
                    //
                }
        }
    }
    //成长印记
    private void GrowthStamp1(int id)
    {
        switch(playercards[id].cardID)
        {
            case 2:
                playercards[id] = (MonsterCard)data.cards[3];
                break;
            case 4:
                playercards[id] = (MonsterCard)data.cards[5];
                break;
            case 10:
                playercards[id] = (MonsterCard)data.cards[11];
                break;
            default:
                playercards[id].attack += 1;
                playercards[id].health += 2;
                playercards[id].cardName = "长毛" + playercards[id].cardName;
                break;
        }
    }
    //成长印记
    private void GrowthStamp2(int id)
    {
        switch (firstlinecards[id].cardID)
        {
            case 2:
                firstlinecards[id] = (MonsterCard)data.cards[3];
                break;
            case 4:
                firstlinecards[id] = (MonsterCard)data.cards[5];
                break;
            case 10:
                firstlinecards[id] = (MonsterCard)data.cards[11];
                break;
            default:
                firstlinecards[id].attack += 1;
                firstlinecards[id].health += 2;
                firstlinecards[id].cardName = "长毛" + firstlinecards[id].cardName;
                break;
        }
    }
    //判断场上是否有阻挡印记的卡牌
    private void FindDefStamp()
    {
        idofDe1 = -1;
        idofDe2 = -1;
        for(int i=0;i<4;i++)
        {
            for(int j=0;j<3;j++)
                if (playercards[i].stamps[j]==Stamp.Defence)
                {
                    idofDe1 = i;
                    break;
                }
            for(int j=0;j<3;j++)
                if (firstlinecards[i].stamps[j]==Stamp.Defence)
                {
                    idofDe2 = i;
                    break;
                }
        }
    }
    //攻击事件
    private void AttackEvent()
    {
        if (!begin)
        {
            for (int i = 0; i < 4; i++)
            {
                int timer = 0;
                while (timer < 800)
                {
                    timer += 1;
                }
                showedpla[i].GetComponent<Attack>().enabled = true;
                Stamps1(i);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                int timer = 0;
                while (timer < 800)
                {
                    timer += 1;
                }
                firshowed[i].GetComponent<Attack>().enabled = true;
                Stamps2(i);
            }
        }
    }
}
