﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace GloomhavenCards
{
  public class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"For {DRAWS} rolls at +{ATK} atk");
      DrawStuff(new Deck(BaseDeck), "BaseDeck");
      Console.WriteLine("- Choices! ----------");
      DrawStuff(new Deck(Minuses), "Minuses");
      DrawStuff(new Deck(ZeroToOne), "ZeroToOne");
      DrawStuff(new Deck(RollinOnes), "RollinOnes");
      DrawStuff(new Deck(AddTarget), "AddTarget");
      DrawStuff(new Deck(Muddle), "Muddle");
      DrawStuff(new Deck(Stun), "Stun");
      Console.WriteLine("- More Choices! ----------");
      DrawStuff(new Deck(AddTarget0to1), "AddTarget0to1");
      DrawStuff(new Deck(AddTargetMinuses), "AddTargetMinuses");
      DrawStuff(new Deck(AddTargetRollin), "AddTargetRollin");


      Console.WriteLine("Press any key to exit");
      Console.ReadKey(true);
    }

    const int DRAWS = 4130;
    const int ATK = 5;

    private static void DrawStuff(Deck theDeck, string name)
    {
      Console.WriteLine($"{name}:");

      DrawAndReport("  Disadvantage: ", theDeck.Disadvantage);
      DrawAndReport("  Normal:       ", theDeck.Draw);
      DrawAndReport("  Advantage:    ", theDeck.Advantage);

      Console.WriteLine();
    }

    private static void DrawAndReport(string style, Func<int, DrawResult> drawStyle)
    {
      var res = new DrawResult[DRAWS];
      Console.Write(style);

      for (int i = 0; i < DRAWS; i++)
        res[i] = drawStyle(ATK);
      var tmp = res.Select(r => (double)r.Value).ToList();

      var stats = res.Where(r => r.Status.Any()).SelectMany(r => r.Status).Distinct().OrderBy(s=>s);

      Console.WriteLine($"Average: {tmp.Mean():F2}, StdDev: {tmp.StandardDeviation():F2} - " +
        string.Join(", ", stats.Select(stat => $"{stat} - {res.Count(r => r.Status.Contains(stat)) * 100.0 / DRAWS:F2} % ")));
    }

    //https://imgur.com/a/w8tTT
    private static readonly List<Card> BaseDeck = new List<Card>
    {
      new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };
    static readonly List<Card> Minuses = new List<Card>
      {
        new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
        new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
        new Card {Value = -1},
        new Card {Value = -2},new Card {Value = 2},
        new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
      };
    private static readonly List<Card> AddTargetMinuses = new List<Card>
    {
      new Card {Value = ATK-2, IsRolling=true}, new Card {Value = ATK-2, IsRolling=true},
      new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };


    static readonly List<Card> ZeroToOne = new List<Card>
      {
        new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
        new Card {Value = 0},new Card {Value = 0},
        new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
        new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
        new Card {Value = -2},new Card {Value = 2},
        new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
      };
    private static readonly List<Card> AddTarget0to1 = new List<Card>
    {
      new Card {Value = ATK-2, IsRolling=true}, new Card {Value = ATK-2, IsRolling=true},
      new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };

    static readonly List<Card> RollinOnes = new List<Card>
      {
        new Card { Value=1, IsRolling=true}, new Card { Value=1, IsRolling=true},
        new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
        new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
        new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
        new Card {Value = -2},new Card {Value = 2},
        new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
      };
    private static readonly List<Card> AddTargetRollin = new List<Card>
    {
      new Card { Value=1, IsRolling=true}, new Card { Value=1, IsRolling=true},
      new Card {Value = ATK-2, IsRolling=true}, new Card {Value = ATK-2, IsRolling=true},
      new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };

    private static readonly List<Card> AddTarget = new List<Card>
    {
      new Card {Value = ATK-2, IsRolling=true}, new Card {Value = ATK-2, IsRolling=true},
      new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };
    private static readonly List<Card> Muddle = new List<Card>
    {
      new Card {Value = 2, Status="Muddle"},
      new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };
    private static readonly List<Card> Stun = new List<Card>
    {
      new Card {Value = 0, Status="Stun"},
      new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},new Card {Value = 0},
      new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},new Card {Value = 1},
      new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},new Card {Value = -1},
      new Card {Value = -2},new Card {Value = 2},
      new Card {Value = 2, IsMultiply = true, IsShuffle=true},new Card {Value = 2, IsMultiply = true, IsShuffle=true}
    };


  }

  public class Card
  {
    public Card()
    {
      Value = 0;
      Status = "";
      IsRolling = false;
      IsShuffle = false;
      IsMultiply = false;
    }
    public int Value { get; set; }
    public string Status { get; set; }
    public bool IsRolling { get; set; }
    public bool IsMultiply { get; set; }
    public bool IsShuffle { get; set; }

    public bool IsBetterThan(Card that)
    {
      if (this.IsMultiply && that.IsMultiply)
        return this.Value > that.Value;
      if (this.IsMultiply)
        return this.Value > 0;
      return this.Value > that.Value; //if gloomhaven doesn't recognize that stun is better than +1, we don't have to either.
    }

  }

  public class Deck
  {
    public Deck(List<Card>  cards)
    {
      Cards = cards;
      Shuffle();
    }

    private readonly Random _R = new Random();
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
      if (rolling.Length + 1 != drawn.Count)
        throw new Exception($"rolling.Count() + 1 = {rolling.Length} drawn.Count = {drawn.Count}!"); //shouldn't happen unless I screwed up
      foreach (var card in rolling)
        result.ApplyCard(card);

      result.ApplyCard(drawn.First(c => !c.IsRolling)); //should be exactly one.

      //we'll never actually get to the end since we have both *0 & *2
      if (drawn.Any(c => c.IsShuffle))
        Shuffle(); //strictly, we wouldn't do this til the round end, but how often to I hit multiple targets anyway.

      return result;
    }

    public DrawResult Advantage(int atk)
    {
      var card1 = Cards[_CurrentIndex++];
      var card2 = Cards[_CurrentIndex++];
      var result = new DrawResult { Value = atk };

      if (!card1.IsRolling && !card2.IsRolling)
      {
        result.ApplyCard(card1.IsBetterThan(card2) ? card1 : card2);
      }
      else if (card1.IsRolling && !card2.IsRolling)
      {
        result.ApplyCard(card1);
        result.ApplyCard(card2);
      }
      else if (!card1.IsRolling && card2.IsRolling)
      {
        result.ApplyCard(card2);
        result.ApplyCard(card1);
      }
      else //2 rollings
      {
        result = Draw(atk); //takes into account further rolling
        result.ApplyCard(card1);
        result.ApplyCard(card2);
      }

      if (card1.IsShuffle || card2.IsShuffle)
        Shuffle();

      return result;
    }

    public DrawResult Disadvantage(int atk)
    {
      var card1 = Cards[_CurrentIndex++];
      var card2 = Cards[_CurrentIndex++];
      var result = new DrawResult { Value = atk };
      var sawMultiply = card1.IsMultiply || card2.IsMultiply;

      if (!card1.IsRolling && !card2.IsRolling)
      {
        result.ApplyCard(card1.IsBetterThan(card2) ? card2 : card1);
      }
      else if (card1.IsRolling && !card2.IsRolling)
      {
        result.ApplyCard(card2);
      }
      else if (!card1.IsRolling && card2.IsRolling)
      {
        result.ApplyCard(card1);
      }
      else //2 rollings
      {
        do
        {
          card1 = Cards[_CurrentIndex++];
          sawMultiply = sawMultiply || card1.IsMultiply;
        } while (card1.IsRolling);
        result.ApplyCard(card1);
      }

      if (sawMultiply)
        Shuffle();

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
