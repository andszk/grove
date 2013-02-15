﻿namespace Grove.Core.Decisions.Human
{
  using System;

  public class SelectCardsPutToHand : Decisions.SelectCardsPutToHand
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}