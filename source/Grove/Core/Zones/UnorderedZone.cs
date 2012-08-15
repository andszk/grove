﻿namespace Grove.Core.Zones
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public abstract class UnorderedZone : IEnumerable<Card>, IHashable, IZone
  {
    private readonly TrackableList<Card> _cards;

    protected UnorderedZone(Game game)
    {
      _cards = new TrackableList<Card>(game.ChangeTracker);
    }

    protected UnorderedZone()
    {
      /* for state copy */
    }

    public int Count { get { return _cards.Count; } }
    public IEnumerable<Card> Creatures { get { return _cards.Where(card => card.Is().Creature); } }

    public bool IsEmpty { get { return _cards.Count == 0; } }
    public IEnumerable<Card> Lands { get { return _cards.Where(card => card.Is().Land); } }

    public Card RandomCard
    {
      get
      {
        var randomIndex = RandomEx.Next(0, _cards.Count - 1);
        return _cards.ElementAt(randomIndex);
      }
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public virtual int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_cards);
    }

    public abstract Zone Zone { get; }

    public virtual void AfterAdd(Card card) {}
    public virtual void AfterRemove(Card card) {}

    public virtual void Add(Card card)
    {
      _cards.Add(card);
      card.ChangeZone(this);
    }

    void IZone.Remove(Card card)
    {
      Remove(card);
    }

    protected virtual void Remove(Card card)
    {
      _cards.Remove(card);
    }

    public void Hide()
    {
      foreach (var card in _cards)
      {
        card.Hide();
      }
    }

    public void Show()
    {
      foreach (var card in _cards)
      {
        card.Show();
      }
    }

    public override string ToString()
    {
      return string.Join(",", _cards.Select(
        card => card.ToString()));
    }
  }
}