using System.Collections.Generic;

public class GameplayState
{
    /* Time */
    public int ActNumber { get; set; } = 0;

    /* Players */
    public Comedian HumanComedian { get; set; }
    public Comedian CpuComedian { get; set; }

    /* Cards */
    public List<Card> HumanDeck { get; } = new List<Card>();
    public List<Card> CpuDeck { get; } = new List<Card>();

    public Queue<CardPlay> CardQueue { get; } = new Queue<CardPlay>();
    public Stack<CardPlay> discardPile { get; } = new Stack<CardPlay>();

    /* "Setup" mechanic that awards bonuses */
    public bool SetupActive { get; set; } = false;
    public ComedyStyle SetupType { get; set; }  // use if bonus dependent on ComedyStyle
    public Comedian SetupPlayer { get; set; }   // who will be awarded the bonus

    /* Dialogue generation data */
    public string CurrentNoun { get; set; }
    public string CurrentAdjective { get; set; }

}