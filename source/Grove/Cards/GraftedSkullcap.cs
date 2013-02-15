﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class GraftedSkullcap : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Grafted Skullcap")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your draw step, draw an additional card.{EOL}At the beginning of your end step, discard your hand.")
        .FlavorText("'Let go your mind. Mine is fitter.'{EOL}—Gix, Yawgmoth praetor")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your draw step, draw an additional card.";
            p.Trigger(new OnStepStart(Step.Draw));
            p.Effect = () => new DrawCards(1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your end step, discard your hand.";
            p.Trigger(new OnStepStart(Step.EndOfTurn));
            p.Effect = () => new DiscardHand();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}