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
    public Stack<CardPlay> DiscardPile { get; } = new Stack<CardPlay>();

    /* "Setup" mechanic that awards bonuses */
    public void ActivateSetup(Comedian player, ComedyStyle style)
    {
        SetupActive = true;
        SetupType = style;
        SetupPlayer = player;
    }
    
    public void DeactivateSetup()
    {
        SetupActive = false;
    }

    public bool SetupActive { get; private set; } = false;
    public ComedyStyle SetupType { get; private set; }  // use if bonus dependent on ComedyStyle
    public Comedian SetupPlayer { get; private set; }   // who will be awarded the bonus


    /* Dialogue generation data */
    public string CurrentNoun { get; set; }
    public string CurrentAdjective { get; set; }

}