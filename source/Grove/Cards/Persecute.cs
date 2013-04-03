﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Persecute : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Persecute")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text("Choose a color. Target player reveals his or her hand and discards all cards of that color.")
        .FlavorText("My finest warrior was lost to the Phyrexians. I pray that Lady Selenia died honorably.")
        .Cast(p =>
          {
            p.Effect = () => new DiscardAllCardsOfChosenColor();
            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new FirstMain());
            p.TargetingRule(new Opponent());
          });

    }
  }
}