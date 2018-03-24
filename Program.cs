using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GloomhavenCards
{
  public class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"For {DRAWS} rolls at +4 atk");
      DrawStuff(new Deck(MyDeck), "MyDeck");
      DrawStuff(new Deck(Less0), "Less0");
      DrawStuff(new Deck(OnesToTwos), "OnesToTwos");
      DrawStuff(new Deck(Icy), "Icy");
      DrawStuff(new Deck(Muddles), "Muddles");
      DrawStuff(new Deck(RollinOnes), "RollinOnes");

      Console.WriteLine("Press any key to exit");
      Console.ReadKey(true);

      //TODO: DrawAdvantage, DrawDisadvantage, display statuses
    }

    const int DRAWS = 413;

    private static void DrawStuff(Deck theDeck, string name)
    {
      Console.WriteLine($"{name}:");

      var res = new DrawResult[DRAWS];

      for (int i = 0; i < DRAWS; i++)
        res[i] = theDeck.Draw(4);
      var tmp = res.Select(r => (double)r.Value).ToList();
      Console.WriteLine($"Average: {tmp.Mean():F2}, StdDev: {tmp.StandardDeviation():F2}");
      Console.WriteLine();
    }

    static List<Card> MyDeck = new List<Card>
      {
        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=0} ,
        new Card { Value=1}, new Card { Value=1},

        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=-1}, new Card { Value=2},
        new Card { Value=1}, new Card { Value=1}, new Card { Value=1},
        new Card { Value=2, IsMultiply=true}, new Card { Value=2, IsMultiply=true}
      };

    static List<Card> Less0 = new List<Card>
      {
        new Card { Value=1}, new Card { Value=1},

        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=-1}, new Card { Value=2},
        new Card { Value=1}, new Card { Value=1}, new Card { Value=1},
        new Card { Value=2, IsMultiply=true}, new Card { Value=2, IsMultiply=true}
      };

    static List<Card> OnesToTwos = new List<Card>
      {
        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=0} ,
        new Card { Value=2}, new Card { Value=2},

        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=-1}, new Card { Value=2},
        new Card { Value=1}, new Card { Value=1}, new Card { Value=1},
        new Card { Value=2, IsMultiply=true}, new Card { Value=2, IsMultiply=true}
      };

    static List<Card> Icy = new List<Card>
      {
        new Card { Value=2, Status="Ice"},
        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=0} ,
        new Card { Value=1}, new Card { Value=1},

        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=-1}, new Card { Value=2},
        new Card { Value=1}, new Card { Value=1}, new Card { Value=1},
        new Card { Value=2, IsMultiply=true}, new Card { Value=2, IsMultiply=true}
      };

    static List<Card> Muddles = new List<Card>
      {
        new Card { Value=0, Status="Muddle", IsRolling=true}, new Card { Value=0, Status="Muddle", IsRolling=true}, new Card { Value=0, Status="Muddle", IsRolling=true},
        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=0} ,
        new Card { Value=1}, new Card { Value=1},

        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=-1}, new Card { Value=2},
        new Card { Value=1}, new Card { Value=1}, new Card { Value=1},
        new Card { Value=2, IsMultiply=true}, new Card { Value=2, IsMultiply=true}
      };

    static List<Card> RollinOnes = new List<Card>
      {
        new Card { Value=1, IsRolling=true}, new Card { Value=1, IsRolling=true},
        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=0} ,
        new Card { Value=1}, new Card { Value=1},

        new Card { Value=0}, new Card { Value=0}, new Card { Value=0}, new Card { Value=-1}, new Card { Value=2},
        new Card { Value=1}, new Card { Value=1}, new Card { Value=1},
        new Card { Value=2, IsMultiply=true}, new Card { Value=2, IsMultiply=true}
      };


  }

  public class Card
  {
    public Card()
    {
      Value = 0;
      Status = "";
      IsRolling = false;
      IsMultiply = false;
    }
    public int Value { get; set; }
    public string Status { get; set; }
    public bool IsRolling { get; set; }
    public bool IsMultiply { get; set; }
  }

  public class Deck
  {
    public Deck(List<Card>  cards)
    {
      Cards = cards;
      Shuffle();
    }

    private Random _R = new Random();
    private int _CurrentIndex;
    private void Shuffle()
    {
      _CurrentIndex = 0;
      for(var i=0; i<Cards.Count; i++)
        Swap(i, _R.Next(i, Cards.Count));
    }

    private void Swap(int i, int j)
    {
      var c = Cards[i];
      Cards[i] = Cards[j];
      Cards[j] = c;
    }

    public DrawResult Draw(int atk)
    {
      var drawn = new List<Card>();

      do
      {
        drawn.Add(Cards[_CurrentIndex++]);
      } while (drawn.Last().IsRolling);

      var result = new DrawResult { Value = atk };

      //rolling
      var rolling = drawn.Where(c => c.IsRolling).ToArray();
      if (rolling.Count() + 1 != drawn.Count)
        throw new Exception($"rolling.Count() + 1 = {rolling.Count()} drawn.Count = {drawn.Count}!"); //shouldn't happen unless I screwed up
      foreach (var card in rolling)
        result.ApplyCard(card);

      result.ApplyCard(drawn.First(c => !c.IsRolling)); //should be exactly one.

      //we'll never actually get to the end since we have both *0 & *2
      if (drawn.Any(c => c.IsMultiply))
        Shuffle(); //strictly, we wouldn't do this til the round end, but how often to I hit multiple targets anyway.

      return result;
    }
    
    public List<Card> Cards { get; }
  }

  public class DrawResult
  {
    public int Value { get; set; }
    public List<string> Status { get; } = new List<string>();

    public void ApplyCard(Card card)
    {
      if (card.IsMultiply)
        Value *= card.Value;
      else
        Value += card.Value;

      if (!string.IsNullOrEmpty(card.Status))
        Status.Add(card.Status);
    }
  }

}
