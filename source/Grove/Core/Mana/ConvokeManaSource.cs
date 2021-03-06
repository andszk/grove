﻿namespace Grove
{
  using System.Collections.Generic;

  public class ConvokeManaSource : IManaSource
  {
    private readonly ManaUnit[] _units;

    public ConvokeManaSource(Card card, int rank = 100)
    {
      OwningCard = card;
      _units = new[]
        {
          new ManaUnit(ManaColor.FromCardColors(card.Colors), rank: rank, source: this)
        };
    }

    public bool CanActivate()
    {
      return !OwningCard.IsTapped;
    }

    public void PayActivationCost()
    {
      OwningCard.Tap();
    }

    public Card OwningCard { get; private set; }

    public IEnumerable<ManaUnit> GetUnits()
    {
      return _units;
    }
  }
}