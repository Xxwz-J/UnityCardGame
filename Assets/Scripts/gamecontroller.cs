using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamecontroller : MonoBehaviour
{
    public GameObject text;
    public GameObject module;
    public GameObject whiteBlock;
    public GameObject panel1;
    public bool isplayerbout; //�Ƿ�����һغ�
    public MonsterCard[] playercards = new MonsterCard[4];
    public GameObject[] showedpla = new GameObject[4];
    public MonsterCard[] allEnemy=new MonsterCard[20]; //��ͨ���˳���˳��   -----------
    public MonsterCard[] anotherEnemy = new MonsterCard[20];//bossս�ڶ��غϵ���    -----------
    public bool[] isEmpty; //��ҷ��Ƿ��г�ս��
    public bool[] isUsed;//�Ƿ�ѡ���׼�
    public Camera uiCamera;
    public int life;//���ʣ����������   ---------------
    public bool isboss;//�Ƿ���bossս  ----------------
    private bool isActive;//bossս�Ƿ����ڶ��غ�
    private int numofRound = 0;//�غ���
    public MonsterCard[] firstlinecards;//��һ�ŵ���
    public GameObject[] firshowed;
    private MonsterCard[] secondlinecards;//�ڶ��ŵ���
    private GameObject[] secshowed;
    private Queue<MonsterCard> firstline=new Queue<MonsterCard>();//��һ�е���
    private Queue<MonsterCard> secondline=new Queue<MonsterCard>();//�ڶ��е���
    private Queue<MonsterCard> thirdline=new Queue<MonsterCard>();//�����е���
    private Queue<MonsterCard> fourthline=new Queue<MonsterCard>();//�����е���
    private bool begin;//����Ƿ񹥻�
    private bool ready;//�з��Ƿ񹥻�
    private int idofDe1; //��ҷ��赲ӡ��ӵ����id
    private int idofDe2; //�з��赲ӡ��ӵ����id
    public int idofThi;
    private int damagePlayerReceived;
    private int damageEnemyReceived;
    private bool gameOver;
    private Vector3 v1 = new Vector3(0, 0, -100);//���λ��1
    private Vector3 v2 = new Vector3(0, -175, -100);//���λ��2
    private Vector2[] positionpla = new Vector2[4];
    public Vector2[] firstv = new Vector2[4];//��һ�ſ���λ��
    private Vector2[] secondv = new Vector2[4];//�ڶ��ſ���λ��
    private Vector2 size = new Vector2(210, 280);
    private CardStore data;
    private int ind = 0;
    private bool[] isGro1 = new bool[4];
    private bool[] isGro2 = new bool[4];
    public bool re;
    public bool reend = false;
    private bool inited = false;
    private bool isMove = false;
    private int moveid;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
        whiteBlock.SetActive(false);
        uiCamera.transform.position = v1;
        isplayerbout = true;
        begin = false;
        ready = false;
        idofDe1 = -1;
        idofDe2 = -1;
        idofThi = -1;
        isEmpty = new bool[4];
        isUsed = new bool[4];
        firstlinecards = new MonsterCard[4];
        firshowed = new GameObject[4];
        secondlinecards = new MonsterCard[4];
        secshowed = new GameObject[4];
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
        positionpla[0] = new Vector2(-520, -140) / 2;
        positionpla[1] = new Vector2(-260, -140) / 2;
        positionpla[2] = new Vector2(0, -140) / 2;
        positionpla[3] = new Vector2(260, -140) / 2;
        firstv[0] = new Vector2(-520, 225) / 2;
        firstv[1] = new Vector2(-260, 225) / 2;
        firstv[2] = new Vector2(0, 225) / 2;
        firstv[3] = new Vector2(260, 225) / 2;
        secondv[0] = new Vector2(-520, 540) / 2;
        secondv[1] = new Vector2(-260, 540) / 2;
        secondv[2] = new Vector2(0, 540) / 2;
        secondv[3] = new Vector2(260, 540) / 2;
        if (isboss)
        {
            if(life==2)
            {
                ShowText(0);
                life = 1;
            }
        }
        data = GetComponent<CardStore>();
        re = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (ind == 1)
                CardMove(0, 1, !begin);
            else CardMove(ind - 1, ind - 2, !begin);
            isMove = false;
            enabled = false;
            return;
        }
        ChangeShowedCard();
        if (ind >= 4)
        {
            int result = damageEnemyReceived - damagePlayerReceived;
            //Debug.Log("result:" + result);
            gameOver = result > 5 || result < -5;
            if (gameOver)
            {
                //Debug.Log("Over");
                if (!begin)
                {
                    if (!isboss || isActive)
                        GameOver(result > 5);
                    else
                    {
                        ReBegin1();
                        ind = 0;
                    }
                }
                else
                    GameOver(result > 5);
            }
            else
            {
                Debug.Log("continue");
                if (!begin)
                {
                    begin = true;
                }
                else
                {
                    ready = true;
                    LoadEnemy();
                }
                ind = 0;
            }
        }
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
        if (inited)
        {
            if (isplayerbout == false && begin == false)
            {
                ChangeCam(1);
                PlayerAttack();
            }
            else if (!isplayerbout && !ready && !re)
            {
                if (firstlinecards[ind] == null)
                {
                    CardMove(ind);
                }
                else if (reend) CardMove(-1);
                else CardMove(-2);
            }
            else if (isplayerbout == false && ready == false)
            {
                EnemyAttack();
                re = false;
                reend = false;
            }
            else if (!isplayerbout)
            {
                AllGro();
                isplayerbout = true;
                begin = false;
                ready = false;
            }
        }
        else
        {
            InitEnemy1();
        }
        //InitEnemy();
    }
    //������ʾ
    private void Buildcard(int line,MonsterCard card,int id)
    {
        if (card != null)
        {
            Transform ChildTr = transform.Find("Panelofplayer");

            GameObject square = Instantiate(module);
            square.GetComponent<CardDisplay>().card = card;

            RectTransform rectTransform = square.GetComponent<RectTransform>();
            if (ChildTr != null)
                square.transform.SetParent(ChildTr, false);
            if (line == 1)
                rectTransform.anchoredPosition = firstv[id] * 2;
            else if (line == 2) rectTransform.anchoredPosition = secondv[id] * 2;
            rectTransform.localScale = new Vector2(0.7f, 0.7f);

            Attack a = square.AddComponent<Attack>();
            a.enabled = false;
            a.id = id;
            a.ispla = false;
            a.contorller = gameObject;
            Shake s = square.AddComponent<Shake>();
            s.enabled = false;
            DelEvent d = square.AddComponent<DelEvent>();
            d.controller = gameObject;
            d.enabled = false;
            square.AddComponent<MoveE>().enabled = false;
            square.GetComponent<MoveE>().controller = gameObject;
            if (line == 1)
                firshowed[id] = square;
            else if (line == 2)
                secshowed[id] = square;

            a.targetPosition1 = positionpla[id];
        }
    }
    //���˿��ƴӵڶ��Ž����һ��
    private void CardMove(int id)
    {
        if (id == -1)
            { return; }
        if (id == -2) { re = true;return; }
        if (secshowed[id] != null)
        {
            enabled = false;
            reend = true;
            MoveE m = secshowed[id].GetComponent<MoveE>();
            m.position = firstv[id] * 2;
            m.enabled = true;
            firstlinecards[id] = secondlinecards[id];
            secondlinecards[id] = null;
            firshowed[id] = secshowed[id];
            secshowed[id] = null;
        }
        else re = true;
    }

    private void CardMove(int id,int target,bool ispla)
    {
        //enabled = false;
        if (!ispla)
        {
            MoveE m = firshowed[id].GetComponent<MoveE>();
            m.position = firstv[target] * 2;
            m.enabled = true;
            firshowed[target] = firshowed[id];
            firshowed[id] = null;
        }
        else
        {
            MoveE m = showedpla[id].GetComponent<MoveE>();
            m.position = positionpla[target] * 2;
            m.enabled = true;
            showedpla[target] = showedpla[id];
            showedpla[id] = null;
            isEmpty[target] = false;
            isEmpty[id] = true;
            Debug.Log("move");
        }
    }
    private void InitEnemy1()
    {
        firstlinecards[0] = new MonsterCard((MonsterCard)data.cards[11]);
        Buildcard(1, firstlinecards[0], 0);
        secondlinecards[1] = new MonsterCard((MonsterCard)data.cards[2]);
        Buildcard(2, secondlinecards[1], 1);
        inited = true;
    }
    //��ʼ���ص�һ�ŵ���
    private void InitEnemy()
    {
        Array.Copy(allEnemy, 0, firstlinecards, 0, 4);
        for(int i=0;i<4;i++)
        {
            Buildcard(1, firstlinecards[i], i);
        }
        if (isboss)
        {
            if (isActive)
            {
                ShowText(2);
                enabled = false;
            }
            else
            {
                ShowText(1);
                enabled = false;
            }
        }
        inited = true;
    }
    //��ҿ��ƹ����׶�
    private void PlayerAttack()
    {
        AttackEvent();
    }
    //������Ϸ�ӽ�
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
    //���˿��ƹ����׶�
    private void EnemyAttack()
    {
        AttackEvent();
    }
    //ÿ�غϼ��صڶ��ŵ���
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
    //��Ϸ����
    private void GameOver(bool playerWin)
    {
        if(playerWin)
        {
            Invoke("VectResult", 1f);
        }
        else
        {
            Invoke("FailedResult", 1f);
        }
        enabled = false;
        SceneManager.LoadScene("cardstore");

    }

    private void VectResult()
    {
        GetComponent<SceneChange1>().LoadSceneWithWhiteFade("AwardScene");
    }

    private void FailedResult()
    {
        if (life == 2)
        {
            life--;
            ShowText(3);
            SceneManager.LoadScene("PlayScenes");
            //���ص�ͼ
        }
        else
        {
            GetComponent<SceneChange1>().LoadSceneWithWhiteFade("FailedScene");
        }
    }
    //�ҷ����Ƽ����ӡ
    private void Stamps1(int id)
    {
        bool isFlying = false;
        bool isFur = false;
        bool isPio = false;
        bool isMot = false;
        bool isDou = false;
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
                    isGro1[id] = true;
                    playercards[id].stamps[i] = Stamp.NullStamp;
                    break;
                case Stamp.Motion:
                    isMot = true;
                    break;
                case Stamp.DoubleAttack:
                    isDou = true;
                    break;
            }
        }
        Attack a = showedpla[id].GetComponent<Attack>();
        int aa = 0;
        if (isDou)
        {
            a.isDou = true;
            aa = 2;
        }
        else
        {
            a.isDou = false;
            aa = 1;
        }
        for (int i = 0; i < aa; i++)
        {
            if (isFur)
            {
                if (id > 0 && id < 3)
                {
                    a.isFur = true;
                    a.targetPosition1 = firstv[id - 1];
                    a.targetPosition2 = firstv[id + 1];
                }
                else if(id==0)
                {
                    a.isFur = true;
                    a.targetPosition1 = firstv[id + 1];
                    a.targetPosition2 = showedpla[id].transform.position;
                }
                else if(id==3)
                {
                    a.isFur = true;
                    a.targetPosition1 = firstv[id - 1];
                    a.targetPosition2 = showedpla[id].transform.position;
                }
                if (id > 0)
                {
                    if (isFlying) damageEnemyReceived += playercards[id].attack;
                    else if (isPio)
                    {
                        if (firstlinecards[id - 1] != null)
                            firstlinecards[id - 1].health = 0;
                        else damageEnemyReceived += playercards[id].attack;
                    }
                    else AttackFront1(playercards[id], id - 1);
                }
                if (id < 3)
                {
                    if (isFlying) damageEnemyReceived += playercards[id].attack;
                    else if (isPio)
                    {
                        if (firstlinecards[id + 1] != null)
                            firstlinecards[id + 1].health = 0;
                        else damageEnemyReceived += playercards[id].attack;
                    }
                    else AttackFront1(playercards[id], id + 1);
                }
            }
            else
            {
                a.targetPosition1 = firstv[id];
                a.targetPosition1.y -= 170;
                if (isFlying) damageEnemyReceived += playercards[id].attack;
                else if (isPio && playercards[ind].attack!=0)
                {
                    if (firstlinecards[id] != null)
                        firstlinecards[id].health = 0;
                    else damageEnemyReceived += playercards[id].attack;
                }
                else
                    AttackFront1(playercards[id], id);
            }
        }
        if (playercards[ind].attack != 0)
        {
            showedpla[ind].GetComponent<Attack>().enabled = true;
            enabled = false;
        }
        if (isMot)
        {
            isMove = true;
            moveid = ind;
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
    //�з����Ƽ����ӡ
    private void Stamps2(int id)
    {
        bool isFlying = false;
        bool isFur = false;
        bool isPio = false;
        bool isMot = false;
        bool isDou = false;
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
                    isGro2[id] = true;
                    firstlinecards[id].stamps[i] = Stamp.NullStamp;
                    break;
                case Stamp.Motion:
                    isMot = true;
                    break;
                case Stamp.DoubleAttack:
                    isDou = true;
                    break;
            }
        }
        Attack a = firshowed[id].GetComponent<Attack>();
        int aa = 0;
        if (isDou)
        {
            a.isDou = true;
            aa = 2;
        }
        else
        {
            a.isDou = false;
            aa = 1;
        }
        for (int i = 0; i < aa; i++)
        {
            if (isFur)
            {
                if (id > 0 && id < 3)
                {
                    a.isFur = true;
                    a.targetPosition1 = positionpla[id - 1];
                    a.targetPosition2 = positionpla[id + 1];
                }
                else if(id==0)
                {
                    a.isFur = false;
                    a.targetPosition1 = positionpla[id+1];
                }
                else if(id==3)
                {
                    a.isFur = false;
                    a.targetPosition1 = positionpla[id - 1];
                }
                if (id > 0)
                {
                    AttackFront2(firstlinecards[id], id - 1);
                }
                if (id < 3)
                {
                    AttackFront2(firstlinecards[id], id + 1);
                }
            }
            else
            {
                a.isFur = false;
                a.targetPosition1 = positionpla[id];
                if (isFlying) damageEnemyReceived += playercards[id].attack;
                else if (isPio)
                {
                    if (firstlinecards[id] != null)
                        firstlinecards[id].health = 0;
                    else damageEnemyReceived += playercards[id].attack;
                }
                else
                    AttackFront2(firstlinecards[id], id);
            }
        }
        if (firstlinecards[ind].attack != 0)
        {
            firshowed[ind].GetComponent<Attack>().enabled = true;
            enabled = false;
        }
        if (isMot)
        {
            isMove = true;
            moveid = ind;
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
    //�ҷ����ƹ���ǰ������
    private void AttackFront1(MonsterCard card,int id)
    {
        if (card.attack != 0)
        {
            if (firstlinecards[id] == null)
                if (idofDe2 != -1)
                {
                    firstlinecards[id] = firstlinecards[idofDe2];
                    firstlinecards[idofDe2] = null;
                    AttackFront1(card, id);
                    return;
                }
                else
                damageEnemyReceived += card.attack;
            else
            {
                if (card.attack > firstlinecards[id].health)
                {
                    if (secondlinecards[id] != null)
                    {
                        if (card.attack - firstlinecards[id].health > secondlinecards[id].health)
                            secondlinecards[id].health = 0;
                        else secondlinecards[id].health -= (card.attack - firstlinecards[id].health);
                    }
                    firstlinecards[id].health = 0;
                }
                else firstlinecards[id].health -= card.attack;
            }
        }
    }
    //�з����ƹ���ǰ��
    private void AttackFront2(MonsterCard card, int id)
    {
        if (card.attack != 0)
        {
            if (playercards[id] == null)
                if(idofDe1!=-1)
                {
                    playercards[id] = playercards[idofDe1];
                    playercards[idofDe1] = null;
                    AttackFront2(card, id);
                    return;
                }
                else
                    damagePlayerReceived += card.attack;
            else
            {
                if (card.attack > playercards[id].health)
                {
                    playercards[id].health = 0;
                }
                else playercards[id].health -= card.attack;
            }
        }
    }
    //�ɳ�ӡ��
    private void GrowthStamp1(int id)
    {
        if (playercards[id].health != 0)
        {
            switch (playercards[id].cardID)
            {
                case 2:
                    MonsterCard newcard1 = new MonsterCard((MonsterCard)data.cards[3]);
                    newcard1.stamps[1] = playercards[id].stamps[1];
                    newcard1.stamps[2] = playercards[id].stamps[2];
                    playercards[id] = newcard1;
                    showedpla[id].GetComponent<CardDisplay>().card = playercards[id];
                    showedpla[id].GetComponent<CardDisplay>().ShowCard();
                    return;
                case 4:
                    MonsterCard newcard2 = new MonsterCard((MonsterCard)data.cards[5]);
                    newcard2.stamps[1] = playercards[id].stamps[1];
                    newcard2.stamps[2] = playercards[id].stamps[2];
                    playercards[id] = newcard2;
                    showedpla[id].GetComponent<CardDisplay>().card = playercards[id];
                    showedpla[id].GetComponent<CardDisplay>().ShowCard();
                    break;
                case 11:
                    MonsterCard newcard3 = new MonsterCard((MonsterCard)data.cards[5]);
                    newcard3.stamps[1] = playercards[id].stamps[1];
                    newcard3.stamps[2] = playercards[id].stamps[2];
                    playercards[id] = newcard3;
                    showedpla[id].GetComponent<CardDisplay>().card = playercards[id];
                    showedpla[id].GetComponent<CardDisplay>().ShowCard();
                    break;
                default:
                    playercards[id].attack += 1;
                    playercards[id].health += 2;
                    playercards[id].cardName = "��ë" + playercards[id].cardName;
                    showedpla[id].GetComponent<CardDisplay>().ShowCard();
                    break;
            }
        }
    }
    //�ɳ�ӡ��
    private void GrowthStamp2(int id)
    {
        if (firstlinecards[id].health != 0)
        {
            switch (firstlinecards[id].cardID)
            {
                case 2:
                    firstlinecards[id] = new MonsterCard((MonsterCard)data.cards[3]);
                    firstlinecards[id].stamps[0] = Stamp.NullStamp;
                    firshowed[id].GetComponent<CardDisplay>().card = firstlinecards[id];
                    firshowed[id].GetComponent<CardDisplay>().ShowCard();
                    break;
                case 4:
                    firstlinecards[id] = new MonsterCard((MonsterCard)data.cards[5]);
                    firstlinecards[id].stamps[0] = Stamp.NullStamp;
                    firshowed[id].GetComponent<CardDisplay>().card = firstlinecards[id];
                    firshowed[id].GetComponent<CardDisplay>().ShowCard();
                    break;
                case 11:
                    firstlinecards[id] = new MonsterCard((MonsterCard)data.cards[13]);
                    firstlinecards[id].stamps[0] = Stamp.NullStamp;
                    firshowed[id].GetComponent<CardDisplay>().card = firstlinecards[id];
                    firshowed[id].GetComponent<CardDisplay>().ShowCard();
                    break;
                default:
                    firstlinecards[id].attack += 1;
                    firstlinecards[id].health += 2;
                    firstlinecards[id].cardName = "��ë" + firstlinecards[id].cardName;
                    firshowed[id].GetComponent<CardDisplay>().ShowCard();
                    break;
            }
        }
    }
    //�жϳ����Ƿ����赲ӡ�ǵĿ���
    private void FindDefStamp()
    {
        idofDe1 = -1;
        idofDe2 = -1;
        for(int i=0;i<4;i++)
        {
            for(int j=0;j<3;j++)
                if (playercards[i]!=null)
                    if (playercards[i].stamps[j]==Stamp.Defence)
                    {
                        idofDe1 = i;
                        break;
                    }
            for(int j=0;j<3;j++)
                if (firstlinecards[i]!=null)
                    if (firstlinecards[i].stamps[j]==Stamp.Defence)
                    {
                        idofDe2 = i;
                        break;
                    }
        }
    }
    //�����¼�
    private void AttackEvent()
    {
        FindDefStamp();
        if (!begin)
        {
            if (playercards[ind] != null)
            {
                Stamps1(ind);
            }
            ind++;
        }
        else
        {
            if (firstlinecards[ind] != null)
            {
                Stamps2(ind);
            }
            ind++;
        }
        //Debug.Log(ind);
    }

    private void ChangeShowedCard()
    {
        for(int i=0;i<4;i++)
        {
            //Debug.Log("show" + i);
            if (showedpla[i] != null)
            {
                showedpla[i].GetComponent<CardDisplay>().ShowCard();
                if (playercards[i].health == 0)
                {
                    playercards[i] = null;
                    showedpla[i].GetComponent<DelEvent>().enabled = true;
                    enabled = false;
                    showedpla[i] = null;
                    //
                    if (i == idofDe1)
                        FindDefStamp();
                    if (isGro1[i])
                        isGro1[i] = false;
                    isEmpty[i] = true;
                }
            }
            if (firshowed[i] != null)
            {
                firshowed[i].GetComponent<CardDisplay>().ShowCard();
                if (firstlinecards[i].health == 0)
                {
                    if (firstlinecards[i].cardID == 15)
                        panel1.GetComponent<playerbout>().GetAward1();
                    firstlinecards[i] = null;
                    firshowed[i].GetComponent<DelEvent>().enabled = true;
                    enabled = false;
                    firshowed[i] = null;
                    //
                    if (i == idofThi)
                        idofThi = -1;
                    if (isGro2[i])
                        isGro2[i] = false;
                    if (i == idofDe2)
                        FindDefStamp();
                }
            }
            if (secshowed[i] != null)
            {
                secshowed[i].GetComponent<CardDisplay>().ShowCard();
                if (secondlinecards[i].health == 0)
                {
                    secondlinecards[i] = null;
                    secshowed[i].GetComponent<DelEvent>().enabled = true;
                    enabled = false;
                    secshowed[i] = null;
                    //
                }
            }
        }
    }
    private void AllGro()
    {
        for(int i=0;i<4;i++)
        {
            if (isGro1[i])
            {
                GrowthStamp1(i);
                isGro1[i] = false;
                //Debug.Log("pla" + i);
            }
            if (isGro2[i])
                { GrowthStamp2(i);
                isGro2[i] = false;
                //Debug.Log("fir" + 1);
            }
        }
    }
    private void ReBegin1()
    {
        isActive = true;
        damageEnemyReceived = 0;
        damagePlayerReceived = 0;
        numofRound = 0;
        allEnemy = anotherEnemy;
        for(int i=0;i<4;i++)
        {
            if (playercards[i]!=null)
            {
                playercards[i] = new MonsterCard((MonsterCard)GetComponent<CardStore>().cards[14]);
                showedpla[i].GetComponent<CardDisplay>().card = playercards[i];
                showedpla[i].GetComponent<CardDisplay>().ShowCard();
            }
            firstlinecards[i] = null;
            if (firshowed[i] != null)
                firshowed[i].GetComponent<DelEvent>().enabled = true;
            firshowed[i] = null;
            secondlinecards[i] = null;
            if (secshowed[i]!=null)
                secshowed[i].GetComponent<DelEvent>().enabled = true;
            secshowed[i] = null;
        }
        firstline = new Queue<MonsterCard>();
        secondline = new Queue<MonsterCard>();
        thirdline = new Queue<MonsterCard>();
        fourthline = new Queue<MonsterCard>();
        allEnemy = anotherEnemy;
        Invoke("ReBegin2", 1f);
    }

    private void ReBegin2()
    {
        firstlinecards[1] = new MonsterCard((MonsterCard)GetComponent<CardStore>().cards[12]);
        Buildcard(1, firstlinecards[1], 1);
        idofThi = 1;
        isplayerbout = true;
        begin = false;
        ready = false;
        panel1.GetComponent<playerbout>().ReBegin();
    }
    public void DefMove(bool ispla,int id)
    {
        if(ispla)
        {
            if (idofDe2 != -1 && firstlinecards[idofDe2] == null)
                {CardMove(idofDe2, id, false);
                idofDe2 = id;
            }
        }
        else
        {
            if (idofDe1 != -1 && playercards[idofDe1] == null)
            {
                CardMove(idofDe1, id, true);
                idofDe1 = id;
            }
        }
    }

    public void ThiMove(int id)
    {
        firstlinecards[id] = firstlinecards[idofThi];
        firstlinecards[idofThi] = null;
        CardMove(idofThi, id, false);
        idofThi = id;
    }

    private void ShowText(int i)
    {
        enabled = false;
        text.SetActive(true);
        TextMeshProUGUI tmpText = text.GetComponent<TextMeshProUGUI>();
        switch (i)
        {
            case 0:
                tmpText.text = "You never lose before.\nThat's your award.";
                Invoke("AwardAction", 1f);
                break;
            case 1:
                tmpText.text = "Have you seen the mule? This's a surprise. \nTry to kill it.";
                break;
            case 2:
                tmpText.text = "He's going to give it his all.\n It'll be over if we win him one more time";
                break;
            case 3:
                tmpText.text = "You lost,but you have another chance.\nDont't lose again.";
                break;
        }
        Invoke("StartEvent", 3f);
    }

    private void StartEvent()
    {
        text.SetActive(false);
        enabled = true;
    }

    private void AwardAction()
    {
        playerbout p = panel1.GetComponent<playerbout>();
        p.GetAward0();
    }
}
