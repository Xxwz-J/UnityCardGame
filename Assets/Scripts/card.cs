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
//Tostring + Update

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

    public override string ToString()
    {
        return attack.ToString() + "," + healthmax.ToString();
    }

    public override void Update(string row)
    {
        string[] rowArray = row.Split(',');
        int attack = int.Parse(rowArray[2]);
        int health = int.Parse(rowArray[3]);
       
    }
}