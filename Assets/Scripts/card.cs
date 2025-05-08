using System;
using Unity.VisualScripting;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Card
{
    public int cardID;

    public string cardName;

    public Card(int cardID, string cardName)
    {
        this.cardID = cardID;
        this.cardName = cardName;
    }

    public Card() {
        this.cardID = 0;
        this.cardName = "ËÉÊó";
    }

    public virtual void Update(string row) { }
    public virtual bool CanGetSacrifice(Card BScard) { return false; }

    public virtual bool CanBeSacrificed(Card GScard) { return false; }
    public override bool Equals(object obj)
    {
        return
            obj is Card card
            && this.cardID == card.cardID
            && this.cardName == card.cardName;
    }
}
public enum Stamp
{
    NullStamp,//¿Õ
    Flying,//·ÉÐÐ
    Furcation,//·Ö²æ¹¥»÷
    DoubleAttack,//Ë«ÖØ¹¥»÷
    Poison,//¶¾ËØ
    Defence,//×èµ²
    Growth,//³É³¤
    Motion,//ÒÆ¶¯
    Thirsty//Ñ°Ñª
}

public class MonsterCard : Card
{
    public int attack;
    public int health;
    public int healthmax;
    public int sacrifice;
    public Stamp[] stamps=new Stamp[3] { Stamp.NullStamp, Stamp.NullStamp, Stamp.NullStamp };
    public bool carved=false;
    public MonsterCard():base(0,"ËÉÊó") {
        this.attack = 0;
        this.health = 1;
        this.healthmax = 1;
        this.sacrifice = 0;
    }
    public MonsterCard(MonsterCard other):base(other.cardID,other.cardName)
    {
        this.attack = other.attack;
        this.health = other.health;
        this.healthmax = other.health;
        this.sacrifice = other.sacrifice;
        Array.Copy(other.stamps, this.stamps, 3);

    }
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
    public override bool CanGetSacrifice(Card BScard) {
        if (BScard is null) return !carved;

        MonsterCard bscard = (MonsterCard)BScard;
        return !carved && bscard.stamps[0] != this.stamps[0];
    }

    public override bool CanBeSacrificed(Card GScard) {
        if (GScard is null)  return !carved && (stamps[0] != Stamp.NullStamp);

        MonsterCard gscard = (MonsterCard)GScard;
        return !carved && (stamps[0] != Stamp.NullStamp) && gscard.stamps[0] != this.stamps[0];

    }
    public override bool Equals(object obj)
    {
        return
            obj is MonsterCard monsterCard
            && base.Equals(obj)
            && this.attack == monsterCard.attack
            && this.healthmax == monsterCard.healthmax
            && this.sacrifice == monsterCard.sacrifice
            && this.stamps[0] == monsterCard.stamps[0]
            && this.stamps[1] == monsterCard.stamps[1]
            && this.stamps[2] == monsterCard.stamps[2];
    }
}