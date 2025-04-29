public class Card
{
    public int cardID;

    public string cardName;

    public Card(int cardID, string cardName)
    {
        this.cardID = cardID;
        this.cardName = cardName;
    }
}


public class MonsterCard : Card
{
    public int attack;
    public int health;
    public int healthmax;
    public int sacrifice;

    public MonsterCard(int cardID, string cardName,int attack, int health, int sacrifice) : base(cardID, cardName)
    {
        this.attack = attack;
        this.health = health;
        this.healthmax = health;
        this.sacrifice = sacrifice;
    }
}