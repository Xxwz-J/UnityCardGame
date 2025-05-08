using System;
using Unity.VisualScripting;

public class Card
{
    public int cardID;

    public string cardName;

    public Card(int cardID, string cardName)
    {
        this.cardID = cardID;
        this.cardName = cardName;
    }

    public virtual void Update(string row) { }
}
public enum Stamp
{
    NullStamp,//¿Õ
    Flying,//·ÉÐÐ
    Furcation,//·Ö²æ¹¥»÷
    Poison,//¶¾ËØ
    Defence,//×èµ²
    Growth,//³É³¤
    Motion//ÒÆ¶¯
}

public class MonsterCard : Card
{
    public int attack;
    public int health;
    public int healthmax;
    public int sacrifice;
    public Stamp[] stamps=new Stamp[3] { Stamp.NullStamp, Stamp.NullStamp, Stamp.NullStamp };
    public bool carved=false;
    public MonsterCard(int cardID, string cardName,int attack, int health, int sacrifice) : base(cardID, cardName)
    {
        this.attack = attack;
        this.health = health;
        this.healthmax = health;
        this.sacrifice = sacrifice;
    }
    public MonsterCard(int cardID, string cardName, int attack, int health, int sacrifice, Stamp stamp) : base(cardID, cardName)
    {
        this.attack = attack;
        this.health = health;
        this.healthmax = health;
        this.sacrifice = sacrifice;
        this.stamps[0] = stamp;
    }
    public MonsterCard(int cardID, string cardName, int attack, int health, int sacrifice, Stamp[] stamps) : base(cardID, cardName)
    {
        this.attack = attack;
        this.health = health;
        this.healthmax = health;
        this.sacrifice = sacrifice;
        this.stamps = stamps;
    }

    public override string ToString()
    {
        return 
            attack.ToString()+"," 
            +healthmax.ToString()+","
            +stamps[0].ToString()+","
            +stamps[1].ToString()+","
            +stamps[2].ToString()+","
            +carved.ToString();
    }

    public override void Update(string row)
    {
        string[] rowArray = row.Split(',');
        attack = int.Parse(rowArray[2]);
        health = int.Parse(rowArray[3]);
        stamps[0] = (Stamp)Enum.Parse(typeof(Stamp), rowArray[4]);
        stamps[1] = (Stamp)Enum.Parse(typeof(Stamp), rowArray[5]);
        stamps[2] = (Stamp)Enum.Parse(typeof(Stamp), rowArray[6]);
        carved = bool.Parse(rowArray[7]);
    }

    public MonsterCard(MonsterCard other) : base(other.cardID, other.cardName)
    {
        this.attack = other.attack;
        this.health = other.health;
        this.healthmax = other.health;
        this.sacrifice = other.sacrifice;
        Array.Copy(other.stamps, this.stamps, 3);

    }

    public int countstamp()
    {
        int a = 0;
        foreach(Stamp stamp in stamps)
        {
            if (stamp != Stamp.NullStamp) a++;
        }
        return a;
    }
}