using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class gamecontroller : MonoBehaviour
{
    public bool isplayerbout;
    public GameObject[] playercards = new GameObject[4];
    public GameObject[] allEnemy;
    public bool[] isEmpty;
    public bool[] isUsed;
    public Camera uiCamera;
    private int numofRound = 0;
    private GameObject[] firstlinecards;
    private GameObject[] secondlinecards;
    private Queue<GameObject> firstline;
    private Queue<GameObject> secondline;
    private Queue<GameObject> thirdline;
    private Queue<GameObject> fourthline;
    private bool begin;
    private bool ready;
    private int damagePlayerReceived;
    private int damageEnemyReceived;
    private bool gameOver;
    private Vector3 v1 = new Vector3(0, 0, -100);
    private Vector3 v2 = new Vector3(0, -170, -100);
    // Start is called before the first frame update
    void Start()
    {
        uiCamera.transform.position = v1;
        isplayerbout = true;
        begin = false;
        ready = false;
        isEmpty = new bool[4];
        isUsed = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            isEmpty[i] = true;
        }
        damageEnemyReceived = 0;
        damagePlayerReceived = 0;
        gameOver = false;
        firstlinecards = new GameObject[4];
        secondlinecards = new GameObject[4];
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
    private void InitEnemy()
    {
        Array.Copy(allEnemy, 0, firstlinecards, 0, 4);
    }
    private void PlayerAttack()
    {
        AttackEvent();
        int result = damageEnemyReceived - damagePlayerReceived;
        gameOver = result > 5 || result < -5;
        if (gameOver)
            GameOver(result > 5);
        else begin = true;
    }

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
    private void EnemyAttack()
    {
        for (int i = 0; i < 4; i++)
            if (firstlinecards[i] == null)
            {
                firstlinecards[i] = secondlinecards[i];
                secondlinecards[i] = null;
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
    private void AttackEvent()
    {
        int timer = 0;
        while(timer<800)
        {
            timer += 1;
        }
        if (!begin)
        {
            for (int i = 0; i < 4; i++)
            {
                //激活刻印
                if (firstlinecards[i] == null)
                    damageEnemyReceived++;  //
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                //激活刻印
                if (playercards[i] == null)
                    damagePlayerReceived++;  //
            }
        }
    }
}
